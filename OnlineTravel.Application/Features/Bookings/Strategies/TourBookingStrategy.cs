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

using Microsoft.Extensions.Logging;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public class TourBookingStrategy : IBookingStrategy
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TourBookingStrategy> _logger;

    public TourBookingStrategy(IUnitOfWork unitOfWork, ILogger<TourBookingStrategy> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public CategoryType Type => CategoryType.Tour;

    public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange? stayRange, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Checking availability for Tour Schedule {ScheduleId}", itemId);

        // Fetch the Specific Schedule first 
        var schedule = await _unitOfWork.Repository<TourSchedule>().GetByIdAsync(itemId, cancellationToken);
        if (schedule == null)
        {
            _logger.LogWarning("Tour Schedule {ScheduleId} not found", itemId);
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Tour Schedule {itemId} was not found."));
        }

        // Fetch the parent Tour
        var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(schedule.TourId, cancellationToken);
        if (tour == null)
        {
            _logger.LogError("Tour not found for Schedule {ScheduleId} (Tour ID: {TourId})", itemId, schedule.TourId);
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Tour for schedule {itemId} was not found."));
        }

        // --- Dynamic Slot Calculation ---
        var activeBookingsSpec = new ActiveBookingDetailsByItemSpec(itemId, DateTime.UtcNow);
        var activeBookingsCount = await _unitOfWork.Repository<BookingDetail>().GetCountAsync(activeBookingsSpec, cancellationToken);
        
        var currentAvailableSlots = schedule.TotalCapacity - activeBookingsCount;

        if (currentAvailableSlots <= 0)
        {
            _logger.LogWarning("Tour '{TourTitle}' schedule is fully booked (Dynamic check: {Count}/{Capacity})", tour.Title, activeBookingsCount, schedule.TotalCapacity);
            return Result<BookingProcessResult>.Failure(Error.Validation("No available slots for this tour schedule."));
        }

        // Atomic reservation check (updates RowVersion via LastReservedAt)
        try 
        {
            schedule.ReserveSlot();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Tour '{TourTitle}' schedule reservation failed: {Message}", tour.Title, ex.Message);
            return Result<BookingProcessResult>.Failure(Error.Validation(ex.Message));
        }

        _unitOfWork.Repository<TourSchedule>().Update(schedule);

        // Calculate Price
        var spec = new TourPriceTierByTourSpec(schedule.TourId);
        var tiers = await _unitOfWork.Repository<TourPriceTier>().GetAllWithSpecAsync(spec, cancellationToken);

        var tier = tiers.FirstOrDefault(t => t.Id == schedule.PriceTierId);

        if (tier == null)
        {
            _logger.LogError("Pricing tier {TierId} not found for Tour '{TourTitle}'", schedule.PriceTierId, tour.Title);
            return Result<BookingProcessResult>.Failure(Error.Validation($"Pricing tier not found for Tour '{tour.Title}' (Tier ID: {schedule.PriceTierId})."));
        }

        var bookedRange = new DateTimeRange(
            schedule.DateRange.Start.ToDateTime(TimeOnly.MinValue),
            schedule.DateRange.End.ToDateTime(TimeOnly.MinValue));

        return Result<BookingProcessResult>.Success(new BookingProcessResult(tier.Price, tour.Title, bookedRange, schedule.Id.ToString()));
    }
}
