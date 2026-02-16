using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Application.Features.Bookings.Strategies;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Bookings.ValueObjects;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.CreateBooking;

public sealed class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly UserManager<AppUser> _userManager;
	private readonly IEnumerable<IBookingStrategy> _bookingStrategies;
	private readonly IMapper _mapper;
	private readonly IPaymentService _paymentService;
	private readonly ILogger<CreateBookingCommandHandler> _logger;

	public CreateBookingCommandHandler(
		IUnitOfWork unitOfWork,
		UserManager<AppUser> userManager,
		IEnumerable<IBookingStrategy> bookingStrategies,
		IMapper mapper,
		IPaymentService paymentService,
		ILogger<CreateBookingCommandHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_userManager = userManager;
		_bookingStrategies = bookingStrategies;
		_mapper = mapper;
		_paymentService = paymentService;
		_logger = logger;
	}

	public async Task<Result<CreateBookingResponse>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Processing booking request for User {UserId}, Item {ItemId}", request.UserId, request.ItemId);

		//  Atomic Database Work 
		BookingEntity booking;
		try
		{
			await using var transaction = await _unitOfWork.BeginTransactionAsync();

			// Validate User
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null)
			{
				_logger.LogWarning("Booking failed: User {UserId} not found", request.UserId);
				return Result<CreateBookingResponse>.Failure(Error.NotFound($"User {request.UserId} was not found."));
			}

			// Validate Category
			var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId);
			if (category == null)
			{
				_logger.LogWarning("Booking failed: Category {CategoryId} not found", request.CategoryId);
				return Result<CreateBookingResponse>.Failure(Error.NotFound($"Category {request.CategoryId} was not found."));
			}

			// Process Booking using Strategy
			var strategy = _bookingStrategies.FirstOrDefault(s => s.Type == category.Type);
			if (strategy == null)
			{
				_logger.LogError("Booking failed: No strategy found for category type {CategoryType}", category.Type);
				return Result<CreateBookingResponse>.Failure(Error.Validation($"No booking strategy found for category type: {category.Type}"));
			}

			var processResult = await strategy.ProcessBookingAsync(request.ItemId, request.StayRange, cancellationToken);
			if (processResult.IsFailure)
			{
				_logger.LogWarning("Booking processing failed: {Error}", processResult.Error.Description);
				return Result<CreateBookingResponse>.Failure(processResult.Error);
			}

			var bookingResult = processResult.Value;
			var totalPrice = bookingResult.TotalPrice;

			// Create Booking
			booking = BookingEntity.Create(request.UserId, totalPrice);

			// Create Booking Detail
			var detail = BookingDetail.Create(request.CategoryId, request.ItemId, bookingResult.ItemName, bookingResult.BookedRange);
			booking.AddDetail(detail);

			// Save and Finalize DB Work
			await _unitOfWork.Repository<BookingEntity>().AddAsync(booking);
			await _unitOfWork.Complete();
			await transaction.CommitAsync();
		}
		catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
		{
			_logger.LogError(ex, "Concurrency conflict during booking creation for Item {ItemId}", request.ItemId);
			return Result<CreateBookingResponse>.Failure(Error.Validation("The item is no longer available. Someone else might have booked it just now. Please try again."));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unexpected error during booking creation for Item {ItemId}", request.ItemId);
			throw;
		}

		_logger.LogInformation("Booking {BookingId} created (Pending). Initializing external payment...", booking.Id);

		//  External Service Call (Outside DB Transaction) 
		var paymentResult = await _paymentService.CreateCheckoutSessionAsync(booking, cancellationToken);
		
		if (paymentResult.IsFailure)
		{
			_logger.LogWarning("Stripe session creation failed for Booking {BookingId}: {Error}", booking.Id, paymentResult.Error.Description);
			
			var responseFail = _mapper.Map<BookingResponse>(booking);
			return Result<CreateBookingResponse>.Success(new CreateBookingResponse { Booking = responseFail }); // Still returning success for the booking, but without URL
		}

		var paymentData = paymentResult.Value;

		// Isolated Persistence Update 
		try 
		{
			booking.UpdateStripeInfo(paymentData.StripeSessionId!, paymentData.PaymentIntentId!);
			await _unitOfWork.Complete();
			
			_logger.LogInformation("Booking {BookingId} metadata updated with Stripe Session {SessionId}", booking.Id, paymentData.StripeSessionId);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to update Booking {BookingId} with Stripe IDs, but session was created.", booking.Id);
		}

		var bookingResponse = _mapper.Map<BookingResponse>(booking);
		return Result<CreateBookingResponse>.Success(new CreateBookingResponse
		{
			Booking = bookingResponse,
			PaymentUrl = paymentData.PaymentUrl
		});
	}
}
