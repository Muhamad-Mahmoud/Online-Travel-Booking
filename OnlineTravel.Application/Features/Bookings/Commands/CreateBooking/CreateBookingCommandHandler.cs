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
using Error = OnlineTravel.Domain.ErrorHandling.Error;

namespace OnlineTravel.Application.Features.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEnumerable<IBookingStrategy> _bookingStrategies;

    public CreateBookingCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IEnumerable<IBookingStrategy> bookingStrategies)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _bookingStrategies = bookingStrategies;
    }

    public async Task<Result<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // Validate User
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result<Guid>.Failure(Error.NotFound($"User {request.UserId} was not found."));

        // Validate Category
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId);
        if (category == null)
            return Result<Guid>.Failure(Error.NotFound($"Category {request.CategoryId} was not found."));

        //  Process Booking using Strategy
        var strategy = _bookingStrategies.FirstOrDefault(s => s.Type == category.Type);
        if (strategy == null)
            return Result<Guid>.Failure(Error.Validation($"No booking strategy found for category type: {category.Type}"));

        var processResult = await strategy.ProcessBookingAsync(request.ItemId, request.StayRange, cancellationToken);
        if (processResult.IsFailure)
            return Result<Guid>.Failure(processResult.Error);

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

        try
        {
            await _unitOfWork.Complete();
            return Result<Guid>.Success(booking.Id);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
        {
            return Result<Guid>.Failure(Error.Validation("The item is no longer available. Someone else might have booked it just now. Please try again."));
        }
    }
}

