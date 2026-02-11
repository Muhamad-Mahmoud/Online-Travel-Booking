using OnlineTravel.Application.Common.Exceptions;
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

        public async Task<Money> CalculateAsync(Guid carId, DateRange stayRange)
        {
            var car = await _unitOfWork.Repository<Car>().GetByIdAsync(carId)
                       ?? throw new NotFoundException(nameof(Car), carId);

            // Validate Availability
            var bookingSpec = new BookingDetailsByItemSpec(carId, stayRange.Start, stayRange.End);
            var existingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(bookingSpec);

            if (existingBookings.Any())
                throw new BadRequestException($"Car {car.Make} {car.Model} is already booked for the selected dates.");

            var spec = new CarPricingTierByCarSpec(carId);
            var carTiers = await _unitOfWork.Repository<CarPricingTier>().GetAllWithSpecAsync(spec);

            if (!carTiers.Any())
                throw new BadRequestException($"No pricing tiers found for Car {car.Make} {car.Model}.");

            var days = stayRange.TotalDays;
            if (days <= 0) days = 1;
            var hours = days * 24;

            // Find tier matching duration
            var tier = carTiers.FirstOrDefault(t => hours >= t.FromHours && hours <= t.ToHours)
                       ?? carTiers.OrderBy(t => t.PricePerHour.Amount).FirstOrDefault(); // Fallback to cheapest/first

            if (tier == null)
                throw new BadRequestException($"Could not determine car pricing tier for {car.Make} {car.Model}.");

            return tier.PricePerHour * hours;
        }
    }
}
