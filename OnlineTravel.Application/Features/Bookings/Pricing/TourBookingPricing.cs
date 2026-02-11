using OnlineTravel.Application.Common.Exceptions;
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

        public async Task<Money> CalculateAsync(Guid tourId, DateRange stayRange)
        {
            var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(tourId)
                       ?? throw new NotFoundException(nameof(Tour), tourId);

            // Validate Schedule & Availability
            var schedule = await _unitOfWork.Repository<TourSchedule>()
                .FindAsync(s => s.TourId == tourId && s.DateRange.Start <= stayRange.Start && s.DateRange.End >= stayRange.End);

            if (schedule == null)
                throw new BadRequestException($"No active tour schedule found for {tour.Title} covering {stayRange}.");

            if (schedule.AvailableSlots <= 0)
                throw new BadRequestException($"Tour '{tour.Title}' is fully booked for the selected dates.");

            // Use the price from the schedule's tier
            var spec = new TourPriceTierByTourSpec(tourId);
            var tiers = await _unitOfWork.Repository<TourPriceTier>().GetAllWithSpecAsync(spec);

            //Ideally we should get price from schedule.PriceTierId, but for now fallback to existing logic or match tier
            var tier = tiers.FirstOrDefault(t => t.Id == schedule.PriceTierId) ?? tiers.FirstOrDefault();

            if (tier == null)
                throw new BadRequestException($"No price tiers found for Tour '{tour.Title}'.");

            return tier.Price;
        }
    }
}
