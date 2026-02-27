using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Infrastructure.Persistence.Context;


namespace OnlineTravel.Infrastructure.Persistence.Repositories
{
	public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
	{
		private readonly OnlineTravelDbContext _context;
		public HotelRepository(OnlineTravelDbContext context) : base(context)
		{
			_context = context;

		}

		public async Task<bool> SlugExistsAsync(string slug)
		{
			return await _context.Hotels.AnyAsync(h => h.Slug == slug.ToLower());
		}

		public async Task<Hotel?> GetWithRoomsAsync(Guid id)
		{
			return await _context.Hotels
				.Include(h => h.Rooms)
					.ThenInclude(r => r.SeasonalPrices)
				.Include(h => h.Rooms)
					.ThenInclude(r => r.RoomAvailabilities)
				.Include(h => h.Reviews)
				.FirstOrDefaultAsync(h => h.Id == id);
		}

		public async Task<Hotel?> GetWithReviewsAsync(Guid id)
		{
			return await _context.Hotels
				.Include(h => h.Reviews)
				.Include(h => h.Rooms)
				.FirstOrDefaultAsync(h => h.Id == id);
		}
	}

}
