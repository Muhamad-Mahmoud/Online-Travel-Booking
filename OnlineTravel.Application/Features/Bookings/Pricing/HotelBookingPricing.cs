using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.BookingSpec;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Bookings.Pricing
{
    public sealed class HotelBookingPricing
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelBookingPricing(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Money>> CalculateAsync(Guid roomId, DateTimeRange stayRange)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(roomId);
            if (room == null)
                return Result<Money>.Failure(Error.NotFound($"Room {roomId} was not found."));

            // Fetch conflicting bookings
            var spec = new BookingDetailsByItemSpec(roomId, DateOnly.FromDateTime(stayRange.Start), DateOnly.FromDateTime(stayRange.End));
            var existingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(spec);
            var conflictingSlots = existingBookings.Select(b => new DateRange(DateOnly.FromDateTime(b.StayRange.Start), DateOnly.FromDateTime(b.StayRange.End)));

            if (!room.IsBookable(stayRange, conflictingSlots))
                return Result<Money>.Failure(Error.Validation($"Room {room.RoomNumber} is currently unavailable for the selected dates."));

            var nights = Math.Max(stayRange.TotalNights, 1);
            return Result<Money>.Success(room.BasePrice * nights);
        }
    }
}
