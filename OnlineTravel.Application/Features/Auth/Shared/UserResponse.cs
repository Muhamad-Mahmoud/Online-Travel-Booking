namespace OnlineTravel.Application.Features.Auth.Shared
{
	public class UserResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public List<string> Roles { get; set; } = new();
	}
}
