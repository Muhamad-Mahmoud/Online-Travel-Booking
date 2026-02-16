using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Hotels
{
    public class SeasonalPrice : BaseEntity
    {
        public Guid RoomId { get; private set; }
        public DateRange DateRange { get; private set; }
        public Money PricePerNight { get; private set; }

        // Navigation property
        public Room Room { get; private set; }

        private SeasonalPrice() { } // EF Core

        public SeasonalPrice(Guid roomId, DateRange dateRange, Money pricePerNight)
        {
            if (roomId == Guid.Empty)
                throw new ArgumentException("Room ID is required", nameof(roomId));

            if (dateRange == null)
                throw new ArgumentNullException(nameof(dateRange));

            if (pricePerNight == null || pricePerNight.Amount <= 0)
                throw new ArgumentException("Price per night must be greater than zero", nameof(pricePerNight));

            Id = Guid.NewGuid();
            RoomId = roomId;
            DateRange = dateRange;
            PricePerNight = pricePerNight;
        }

    }
}
