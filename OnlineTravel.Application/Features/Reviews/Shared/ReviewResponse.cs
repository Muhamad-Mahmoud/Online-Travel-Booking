using OnlineTravel.Domain.Entities.Reviews;

namespace OnlineTravel.Application.Features.Reviews.Shared
{
	public class ReviewResponse
	{
		public Guid Id { get; set; }
		public string Comment { get; set; } = string.Empty;
		public int Rating { get; set; }
		public string UserName { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}
}
