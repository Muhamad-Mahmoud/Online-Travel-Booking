using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.Common.Exceptions;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Bookings.ValueObjects;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Application.Features.Bookings.Strategies;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.DTOs;

using Microsoft.Extensions.Logging;

namespace OnlineTravel.Application.Features.Bookings.CreateBooking;

public sealed class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<BookingResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEnumerable<IBookingStrategy> _bookingStrategies;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBookingCommandHandler> _logger;

    public CreateBookingCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IEnumerable<IBookingStrategy> bookingStrategies,
        IMapper mapper,
        ILogger<CreateBookingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _bookingStrategies = bookingStrategies;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<BookingResponse>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing booking request for User {UserId}, Item {ItemId}", request.UserId, request.ItemId);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Validate User
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                _logger.LogWarning("Booking failed: User {UserId} not found", request.UserId);
                return Result<BookingResponse>.Failure(Error.NotFound($"User {request.UserId} was not found."));
            }

            // Validate Category
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Booking failed: Category {CategoryId} not found", request.CategoryId);
                return Result<BookingResponse>.Failure(Error.NotFound($"Category {request.CategoryId} was not found."));
            }

            //  Process Booking using Strategy
            var strategy = _bookingStrategies.FirstOrDefault(s => s.Type == category.Type);
            if (strategy == null)
            {
                _logger.LogError("Booking failed: No strategy found for category type {CategoryType}", category.Type);
                return Result<BookingResponse>.Failure(Error.Validation($"No booking strategy found for category type: {category.Type}"));
            }

            var processResult = await strategy.ProcessBookingAsync(request.ItemId, request.StayRange, cancellationToken);
            if (processResult.IsFailure)
            {
                _logger.LogWarning("Booking processing failed: {Error}", processResult.Error.Description);
                return Result<BookingResponse>.Failure(processResult.Error);
            }

            var bookingResult = processResult.Value;
            var totalPrice = bookingResult.TotalPrice;

            // Create Booking Reference
            var reference = new BookingReference($"BK-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}");

            // Create Booking
            var booking = BookingEntity.Create(
                request.UserId,
                reference,
                totalPrice
            );

            // Create Booking Detail
            var detail = BookingDetail.Create(
                request.CategoryId,
                request.ItemId,
                request.StayRange
            );

            booking.AddDetail(detail);

            // Save to DB (Commit Transaction)
            await _unitOfWork.Repository<BookingEntity>().AddAsync(booking);
            await _unitOfWork.Complete();
            
            await _unitOfWork.CommitTransactionAsync();

            var response = _mapper.Map<BookingResponse>(booking);
            _logger.LogInformation("Booking {BookingId} created successfully for User {UserId}", booking.Id, request.UserId);
            return Result<BookingResponse>.Success(response);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Concurrency conflict during booking creation for Item {ItemId}", request.ItemId);
            return Result<BookingResponse>.Failure(Error.Validation("The item is no longer available. Someone else might have booked it just now. Please try again."));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Unexpected error during booking creation for Item {ItemId}", request.ItemId);
            throw; // Re-throw to let middleware handle it
        }
    }
}

