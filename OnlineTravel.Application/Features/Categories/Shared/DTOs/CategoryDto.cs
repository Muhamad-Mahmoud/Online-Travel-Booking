using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Categories.Shared.DTOs
{
	public class CategoryDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public CategoryType Type { get; set; }
	}
}
