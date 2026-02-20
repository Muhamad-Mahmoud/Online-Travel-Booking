using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Hotels
{
    public class RoomAvailability : BaseEntity
    {
        public Guid RoomId { get; private set; }
        public DateRange DateRange { get; private set; }
        public bool IsAvailable { get; private set; }

        // Navigation property
        public Room Room { get; private set; }

        private RoomAvailability() { } // EF Core

        public RoomAvailability(Guid roomId, DateRange dateRange, bool isAvailable)
        {
            if (roomId == Guid.Empty)
                throw new ArgumentException("Room ID is required", nameof(roomId));

            if (dateRange == null)
                throw new ArgumentNullException(nameof(dateRange));

            Id = Guid.NewGuid();
            RoomId = roomId;
            DateRange = dateRange;
            IsAvailable = isAvailable;
        }

    }
}
