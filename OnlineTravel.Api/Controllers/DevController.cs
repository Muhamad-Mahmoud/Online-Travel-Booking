using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Infrastructure.Persistence.Context;

namespace OnlineTravel.Api.Controllers
{
	[ApiController]
	[Route("api/v1/dev")]
	[Authorize(Roles = "Admin")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class DevController : ControllerBase
	{
		private readonly OnlineTravelDbContext _context;

		public DevController(OnlineTravelDbContext context)
		{
			_context = context;
		}

		[HttpGet("seed-data")]
		public async Task<IActionResult> GetSeedData()
		{
			var customer = await _context.Users.FirstOrDefaultAsync(u => u.Email == "customer@onlinetravel.com");
			var categories = await _context.Categories.ToListAsync();
			var hotels = await _context.Hotels.Include(h => h.Rooms).Take(1).ToListAsync();
			var tours = await _context.Tours.Take(1).ToListAsync();

			return Ok(new
			{
				Customer = new { customer?.Id, customer?.Email },
				Categories = categories.Select(c => new { c.Id, c.Title, c.Type }),
				SampleHotel = hotels.Select(h => new { h.Id, h.Name, RoomId = h.Rooms.FirstOrDefault()?.Id }),
				SampleTour = tours.Select(t => new { t.Id, t.Title })
			});
		}
	}
}

