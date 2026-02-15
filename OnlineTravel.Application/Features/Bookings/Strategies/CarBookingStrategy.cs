using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Application.Specifications.BookingSpec;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public class CarBookingStrategy : IBookingStrategy
{
    private readonly IUnitOfWork _unitOfWork;

    public CarBookingStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public CategoryType Type => CategoryType.Car;

    public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange stayRange, CancellationToken cancellationToken)
    {
        var car = await _unitOfWork.Repository<Car>().GetByIdAsync(itemId, cancellationToken);
        if (car == null)
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Car {itemId} was not found."));

        // Validate Availability against Booking Table 
        var overlappingSpec = new OverlappingBookingDetailsSpec(itemId, DateOnly.FromDateTime(stayRange.Start), DateOnly.FromDateTime(stayRange.End));
        var overlappingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(overlappingSpec, cancellationToken);

        if (overlappingBookings.Any())
            return Result<BookingProcessResult>.Failure(Error.Validation($"Car {car.Make} {car.Model} is already booked for the selected dates."));

        // Touch the entity to trigger RowVersion check on Save
        _unitOfWork.Repository<Car>().Update(car);

        var spec = new CarPricingTierByCarSpec(itemId);
        var carTiers = await _unitOfWork.Repository<CarPricingTier>().GetAllWithSpecAsync(spec, cancellationToken);

        if (!carTiers.Any())
            return Result<BookingProcessResult>.Failure(Error.Validation($"No pricing tiers found for Car {car.Make} {car.Model}."));

        var hours = Math.Ceiling(stayRange.TotalHours);
        if (hours <= 0)
            return Result<BookingProcessResult>.Failure(Error.Validation("Invalid booking duration. Minimum duration is 1 hour."));

        // Find tier matching duration
        var tier = carTiers.FirstOrDefault(t => hours >= t.FromHours && hours <= t.ToHours);

        if (tier == null)
            return Result<BookingProcessResult>.Failure(Error.Validation($"No pricing tier matches the selected duration ({hours} hours) for {car.Make} {car.Model}."));

        var totalPrice = tier.PricePerHour * (decimal)hours;
        return Result<BookingProcessResult>.Success(new BookingProcessResult(totalPrice, car.Id.ToString()));
    }
}
