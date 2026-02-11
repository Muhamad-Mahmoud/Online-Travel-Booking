using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.Common.Exceptions;
using OnlineTravel.Application.Features.Bookings.Pricing;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Bookings.ValueObjects;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;

namespace OnlineTravel.Application.Features.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly HotelBookingPricing _hotelPricing;
    private readonly TourBookingPricing _tourPricing;
    private readonly FlightBookingPricing _flightPricing;
    private readonly CarBookingPricing _carPricing;

    public CreateBookingCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        HotelBookingPricing hotelPricing,
        TourBookingPricing tourPricing,
        FlightBookingPricing flightPricing,
        CarBookingPricing carPricing)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _hotelPricing = hotelPricing;
        _tourPricing = tourPricing;
        _flightPricing = flightPricing;
        _carPricing = carPricing;
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

        //  Calculate Price & Validate Availability
        Result<Money> priceResult;

        switch (category.Type)
        {
            case CategoryType.Hotel:
                priceResult = await _hotelPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            case CategoryType.Tour:
                priceResult = await _tourPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            case CategoryType.Flight:
                priceResult = await _flightPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            case CategoryType.Car:
                priceResult = await _carPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            default:
                return Result<Guid>.Failure(Error.Validation($"Unsupported category type: {category.Type}"));
        }

        if (priceResult.IsFailure)
            return Result<Guid>.Failure(priceResult.Error);

        var totalPrice = priceResult.Value;

        // Create Booking
        var reference = new BookingReference($"BK-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}");

        var booking = BookingEntity.Create(
            request.UserId,
            reference,
            totalPrice
        );

        // Update Inventory
        if (category.Type == CategoryType.Tour)
        {
            var startDate = DateOnly.FromDateTime(request.StayRange.Start);
            var endDate = DateOnly.FromDateTime(request.StayRange.End);

            var schedule = await _unitOfWork.Repository<TourSchedule>()
                .FindAsync(s => s.TourId == request.ItemId && s.DateRange.Start <= startDate && s.DateRange.End >= endDate);

            if (schedule != null)
            {
                schedule.AvailableSlots -= 1;
                if (schedule.AvailableSlots < 0)
                    return Result<Guid>.Failure(Error.Validation("Tour is fully booked."));
                _unitOfWork.Repository<TourSchedule>().Update(schedule);
            }
        }
        else if (category.Type == CategoryType.Flight)
        {
            var seatSpec = new Specifications.BaseSpecification<FlightSeat>(s => s.FlightId == request.ItemId && s.IsAvailable);
            seatSpec.Take = 1;
            seatSpec.IsPaginationEnabled = true;

            var seats = await _unitOfWork.Repository<FlightSeat>().GetAllWithSpecAsync(seatSpec);
            var seat = seats.FirstOrDefault();

            if (seat != null)
            {
                seat.IsAvailable = false;
                _unitOfWork.Repository<FlightSeat>().Update(seat);
            }
        }

        // Create Booking Detail
        var detail = BookingDetail.Create(
            request.CategoryId,
            request.ItemId,
            request.StayRange
        );

        booking.AddDetail(detail);

        // Save to DB
        await _unitOfWork.Repository<BookingEntity>().AddAsync(booking);
        await _unitOfWork.Complete();

        return Result<Guid>.Success(booking.Id);
    }
}

