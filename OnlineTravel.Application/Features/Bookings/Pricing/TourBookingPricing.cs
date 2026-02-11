using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Bookings.Pricing
{
    public class TourBookingPricing
    {
        private readonly IUnitOfWork _unitOfWork;

        public TourBookingPricing(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Money>> CalculateAsync(Guid tourId, DateTimeRange stayRange)
        {
            var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(tourId);
            if (tour == null)
                return Result<Money>.Failure(Error.NotFound($"Tour {tourId} was not found."));

            // Validate Schedule & Availability
            var startDate = DateOnly.FromDateTime(stayRange.Start);
            var endDate = DateOnly.FromDateTime(stayRange.End);

            var schedule = await _unitOfWork.Repository<TourSchedule>()
                .FindAsync(s => s.TourId == tourId && s.DateRange.Start <= startDate && s.DateRange.End >= endDate);

            if (schedule == null)
                return Result<Money>.Failure(Error.Validation($"The tour '{tour.Title}' is not scheduled for the selected dates ({stayRange}). Please check the available dates on the tour page."));

            if (schedule.AvailableSlots <= 0)
                return Result<Money>.Failure(Error.Validation($"Tour '{tour.Title}' is fully booked for the selected dates."));

            // Use the price from the schedule's tier
            var spec = new TourPriceTierByTourSpec(tourId);
            var tiers = await _unitOfWork.Repository<TourPriceTier>().GetAllWithSpecAsync(spec);

            //Ideally we should get price from schedule.PriceTierId
            var tier = tiers.FirstOrDefault(t => t.Id == schedule.PriceTierId);

            if (tier == null)
                return Result<Money>.Failure(Error.Validation($"Pricing tier not found for Tour '{tour.Title}' (Tier ID: {schedule.PriceTierId})."));

            return Result<Money>.Success(tier.Price);
        }
    }
}
