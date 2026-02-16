using Microsoft.Extensions.Logging;
using OnlineTravel.Application.Features.Bookings.Specifications.Availability;
using OnlineTravel.Application.Features.Bookings.Specifications.Pricing;
using OnlineTravel.Application.Interfaces.Persistence;
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
	private readonly ILogger<CarBookingStrategy> _logger;

	public CarBookingStrategy(IUnitOfWork unitOfWork, ILogger<CarBookingStrategy> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public CategoryType Type => CategoryType.Car;

	public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange? stayRange, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Checking availability for Car {CarId}", itemId);

        if (stayRange == null)
        {
            return Result<BookingProcessResult>.Failure(Error.Validation("Stay range is required for car bookings."));
        }

		var car = await _unitOfWork.Repository<Car>().GetByIdAsync(itemId, cancellationToken);
		if (car == null)
		{
			_logger.LogWarning("Car {CarId} not found", itemId);
			return Result<BookingProcessResult>.Failure(Error.NotFound($"Car {itemId} was not found."));
		}

		// Validate Availability against Booking Table 
		var overlappingSpec = new OverlappingBookingDetailsSpec(itemId, stayRange.Start, stayRange.End, DateTime.UtcNow);
		var overlappingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(overlappingSpec, cancellationToken);

		if (overlappingBookings.Any())
		{
			_logger.LogWarning("Car {CarModel} is already booked", car.Model);
			return Result<BookingProcessResult>.Failure(Error.Validation($"Car {car.Make} {car.Model} is already booked for the selected dates."));
		}

		// and triggers optimistic concurrency via RowVersion 
		car.Reserve();
		_unitOfWork.Repository<Car>().Update(car);

		var spec = new CarPricingTierByCarSpec(itemId);
		var carTiers = await _unitOfWork.Repository<CarPricingTier>().GetAllWithSpecAsync(spec, cancellationToken);

		if (!carTiers.Any())
		{
			_logger.LogError("No pricing tiers found for Car {CarModel}", car.Model);
			return Result<BookingProcessResult>.Failure(Error.Validation($"No pricing tiers found for Car {car.Make} {car.Model}."));
		}

		var hours = Math.Ceiling(stayRange.TotalHours);
		if (hours <= 0)
			return Result<BookingProcessResult>.Failure(Error.Validation("Invalid booking duration. Minimum duration is 1 hour."));

		// Find tier matching duration
		var tier = carTiers.FirstOrDefault(t => hours >= t.FromHours && hours <= t.ToHours);

		if (tier == null)
		{
			_logger.LogWarning("No pricing tier matches duration {Duration} hours for Car {CarModel}", hours, car.Model);
			return Result<BookingProcessResult>.Failure(Error.Validation($"No pricing tier matches the selected duration ({hours} hours) for {car.Make} {car.Model}."));
		}

		var totalPrice = tier.PricePerHour * (decimal)hours;
		return Result<BookingProcessResult>.Success(new BookingProcessResult(totalPrice, $"{car.Make} {car.Model}", stayRange));
	}
}
