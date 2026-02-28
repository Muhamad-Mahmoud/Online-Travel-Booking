using OnlineTravel.Application.Features.Auth.Shared;

namespace OnlineTravel.Application.Features.Auth.Login
{
	public class AuthResponse
	{
		public bool IsSuccess { get; set; }
		public string? Message { get; set; }

		public string? Token { get; set; }
		public DateTime? ExpiresAt { get; set; }

		public UserResponse? User { get; set; }
	}

}
