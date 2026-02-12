using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.BookingSpec;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public class HotelBookingStrategy : IBookingStrategy
{
    private readonly IUnitOfWork _unitOfWork;

    public HotelBookingStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public CategoryType Type => CategoryType.Hotel;

    public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange stayRange, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Repository<Room>().GetByIdAsync(itemId, cancellationToken);
        if (room == null)
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Room {itemId} was not found."));

        // Fetch conflicting bookings from the Booking table
        var overlappingSpec = new OverlappingBookingDetailsSpec(itemId, DateOnly.FromDateTime(stayRange.Start), DateOnly.FromDateTime(stayRange.End));
        var overlappingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(overlappingSpec, cancellationToken);

        if (overlappingBookings.Any())
            return Result<BookingProcessResult>.Failure(Error.Validation($"Room {room.RoomNumber} is already booked for the selected dates."));

        // Touch the entity to trigger RowVersion check on Save
        _unitOfWork.Repository<Room>().Update(room);

        var nights = Math.Max(stayRange.TotalNights, 1);
        var totalPrice = room.BasePrice * nights;

        return Result<BookingProcessResult>.Success(new BookingProcessResult(totalPrice, room.Id.ToString()));
    }
}
