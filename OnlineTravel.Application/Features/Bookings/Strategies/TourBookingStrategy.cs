using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Application.Features.Bookings.Specifications.Pricing;
using OnlineTravel.Application.Features.Bookings.Specifications.Availability;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public class TourBookingStrategy : IBookingStrategy
{
    private readonly IUnitOfWork _unitOfWork;

    public TourBookingStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public CategoryType Type => CategoryType.Tour;

    public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange stayRange, CancellationToken cancellationToken)
    {
        // Fetch the Specific Schedule first 
        var schedule = await _unitOfWork.Repository<TourSchedule>().GetByIdAsync(itemId, cancellationToken);
        if (schedule == null)
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Tour Schedule {itemId} was not found."));

        // Fetch the parent Tour
        var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(schedule.TourId, cancellationToken);
        if (tour == null)
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Tour for schedule {itemId} was not found."));

        // Check for slots availability 
        var overlappingSpec = new OverlappingBookingDetailsSpec(itemId, DateOnly.FromDateTime(stayRange.Start), DateOnly.FromDateTime(stayRange.End), DateTime.UtcNow);
        var overlappingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(overlappingSpec, cancellationToken);

        if (overlappingBookings.Count() >= schedule.TotalCapacity)
            return Result<BookingProcessResult>.Failure(Error.Validation($"The tour '{tour.Title}' for this schedule is fully booked."));

        schedule.LastReservedAt = DateTime.UtcNow;
        _unitOfWork.Repository<TourSchedule>().Update(schedule);

        // Calculate Price
        var spec = new TourPriceTierByTourSpec(schedule.TourId);
        var tiers = await _unitOfWork.Repository<TourPriceTier>().GetAllWithSpecAsync(spec, cancellationToken);

        var tier = tiers.FirstOrDefault(t => t.Id == schedule.PriceTierId);

        if (tier == null)
            return Result<BookingProcessResult>.Failure(Error.Validation($"Pricing tier not found for Tour '{tour.Title}' (Tier ID: {schedule.PriceTierId})."));

        return Result<BookingProcessResult>.Success(new BookingProcessResult(tier.Price, schedule.Id.ToString()));
    }
}
