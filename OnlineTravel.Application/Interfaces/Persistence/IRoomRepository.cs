using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<Room?> GetWithAvailabilityAsync(Guid id);
        Task<Room?> GetWithSeasonalPricesAsync(Guid id);
        Task<IReadOnlyList<Room>> GetHotelRoomsAsync(Guid hotelId);
    }

}
