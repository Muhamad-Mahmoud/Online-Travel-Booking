using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Infrastructure.Persistence.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly OnlineTravelDbContext _context;
        public RoomRepository(OnlineTravelDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<Room?> GetWithAvailabilityAsync(Guid id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.RoomAvailabilities)
                .Include(r => r.Bookings)
                .Include(r => r.SeasonalPrices)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room?> GetWithSeasonalPricesAsync(Guid id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.SeasonalPrices)
                .Include(r => r.RoomAvailabilities)
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> RoomNumberExistsInHotelAsync(Guid hotelId, string roomNumber)
        {
            return await _context.Rooms
                .AnyAsync(r => r.HotelId == hotelId && r.RoomNumber == roomNumber);
        }

        public async Task<IReadOnlyList<Room>> GetHotelRoomsAsync(Guid hotelId)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.SeasonalPrices)
                .Include(r => r.RoomAvailabilities)
                .Include(r => r.Bookings)
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
        }
    }


}
