using OnlineTravel.Domain.Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<Room?> GetWithAvailabilityAsync(Guid id);
        Task<Room?> GetWithSeasonalPricesAsync(Guid id);
        Task<bool> RoomNumberExistsInHotelAsync(Guid hotelId, string roomNumber);
        Task<IReadOnlyList<Room>> GetHotelRoomsAsync(Guid hotelId);
    }

}
