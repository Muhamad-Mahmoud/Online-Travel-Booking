namespace OnlineTravel.Application.Features.Hotels.Dtos
{
	public class ReviewDto
	{
		public Guid Id { get; set; }
		public Guid HotelId { get; set; }
		public string UserId { get; set; } = string.Empty;
		public int Rating { get; set; }
		public string Comment { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}

}
