using OnlineTravel.Application.Common.Exceptions;
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

        public async Task<Money> CalculateAsync(Guid roomId, DateRange stayRange)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(roomId)
                       ?? throw new NotFoundException(nameof(Room), roomId);

            // Fetch conflicting bookings
            var spec = new BookingDetailsByItemSpec(roomId, stayRange.Start, stayRange.End);
            var existingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(spec);
            var conflictingSlots = existingBookings.Select(b => b.StayRange);

            if (!room.IsBookable(stayRange, conflictingSlots))
                throw new BadRequestException($"Room {room.RoomNumber} is currently unavailable for the selected dates.");

            var nights = Math.Max(stayRange.TotalNights, 1);
            return room.BasePrice * nights;
        }
    }
}
