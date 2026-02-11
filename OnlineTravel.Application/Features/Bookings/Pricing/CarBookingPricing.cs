using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Application.Specifications.BookingSpec;

namespace OnlineTravel.Application.Features.Bookings.Pricing
{
    public class CarBookingPricing
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarBookingPricing(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Money>> CalculateAsync(Guid carId, DateTimeRange stayRange)
        {
            var car = await _unitOfWork.Repository<Car>().GetByIdAsync(carId);
            if (car == null)
                return Result<Money>.Failure(Error.NotFound($"Car {carId} was not found."));

            // Validate Availability
            var bookingSpec = new BookingDetailsByItemSpec(carId, DateOnly.FromDateTime(stayRange.Start), DateOnly.FromDateTime(stayRange.End));
            var existingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(bookingSpec);

            if (existingBookings.Any())
                return Result<Money>.Failure(Error.Validation($"Car {car.Make} {car.Model} is already booked for the selected dates."));

            var spec = new CarPricingTierByCarSpec(carId);
            var carTiers = await _unitOfWork.Repository<CarPricingTier>().GetAllWithSpecAsync(spec);

            if (!carTiers.Any())
                return Result<Money>.Failure(Error.Validation($"No pricing tiers found for Car {car.Make} {car.Model}."));

            var hours = Math.Ceiling(stayRange.TotalHours);
            if (hours <= 0)
                return Result<Money>.Failure(Error.Validation("Invalid booking duration. Minimum duration is 1 hour."));

            // Find tier matching duration
            var tier = carTiers.FirstOrDefault(t => hours >= t.FromHours && hours <= t.ToHours);

            if (tier == null)
                return Result<Money>.Failure(Error.Validation($"No pricing tier matches the selected duration ({hours} hours) for {car.Make} {car.Model}."));

            return Result<Money>.Success(tier.PricePerHour * (decimal)hours);
        }
    }
}
