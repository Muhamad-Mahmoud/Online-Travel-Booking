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

namespace OnlineTravel.Application.Features.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly HotelBookingPricing _hotelPricing;
    private readonly TourBookingPricing _tourPricing;
    private readonly FlightBookingPricing _flightPricing;
    private readonly CarBookingPricing _carPricing;

    public CreateBookingCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
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

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // Validate User
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new NotFoundException(nameof(User), request.UserId);

        // Validate Category
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        //  Calculate Price & Validate Availability
        Money totalPrice;

        switch (category.Type)
        {
            case CategoryType.Hotel:
                totalPrice = await _hotelPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            case CategoryType.Tour:
                totalPrice = await _tourPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            case CategoryType.Flight:
                totalPrice = await _flightPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            case CategoryType.Car:
                totalPrice = await _carPricing.CalculateAsync(request.ItemId, request.StayRange);
                break;
            default:
                throw new BadRequestException($"Unsupported category type: {category.Type}");
        }

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
            var schedule = await _unitOfWork.Repository<TourSchedule>()
                .FindAsync(s => s.TourId == request.ItemId && s.DateRange.Start <= request.StayRange.Start && s.DateRange.End >= request.StayRange.End);

            if (schedule != null)
            {
                schedule.AvailableSlots -= 1;
                if (schedule.AvailableSlots < 0) throw new BadRequestException("Tour is fully booked.");
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

        return booking.Id;
    }
}

