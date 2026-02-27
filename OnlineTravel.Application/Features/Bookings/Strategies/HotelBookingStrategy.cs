using Microsoft.Extensions.Logging;
using OnlineTravel.Application.Features.Bookings.Specifications.Availability;
using OnlineTravel.Application.Interfaces.Persistence;
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
	private readonly ILogger<HotelBookingStrategy> _logger;

	public HotelBookingStrategy(IUnitOfWork unitOfWork, ILogger<HotelBookingStrategy> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public CategoryType Type => CategoryType.Hotel;

	public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange? stayRange, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Checking availability for Room {RoomId}", itemId);

		if (stayRange == null)
		{
			return Result<BookingProcessResult>.Failure(Error.Validation("Stay range is required for hotel bookings."));
		}

		_logger.LogDebug("Checking availability for Room {RoomId} from {Start} to {End}", itemId, stayRange.Start, stayRange.End);

		var room = await _unitOfWork.Repository<Room>().GetByIdAsync(itemId, cancellationToken);
		if (room == null)
		{
			_logger.LogWarning("Room {RoomId} not found", itemId);
			return Result<BookingProcessResult>.Failure(Error.NotFound($"Room {itemId} was not found."));
		}

		// Fetch conflicting bookings from the Booking table
		var overlappingSpec = new OverlappingBookingDetailsSpec(itemId, stayRange.Start, stayRange.End, DateTime.UtcNow);
		var overlappingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(overlappingSpec, cancellationToken);

		if (overlappingBookings.Any())
		{
			_logger.LogWarning("Room {RoomNumber} is already booked for dates {Start}-{End}", room.RoomNumber, stayRange.Start, stayRange.End);
			return Result<BookingProcessResult>.Failure(Error.Validation($"Room {room.RoomNumber} is already booked for the selected dates."));
			// return Result<BookingProcessResult>.Failure(Error.Validation($"Room {room.Name} is already booked for the selected dates."));
		}

		room.Reserve();
		_unitOfWork.Repository<Room>().Update(room);

		var nights = Math.Max(stayRange.TotalNights, 1);
		var totalPrice = room.BasePricePerNight * nights;
		var itemName = $"{room.Name} - Room {room.RoomNumber}";

		return Result<BookingProcessResult>.Success(new BookingProcessResult(totalPrice, itemName, stayRange, room.Id.ToString()));

	}
}
