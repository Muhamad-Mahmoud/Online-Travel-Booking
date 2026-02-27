using OnlineTravel.Application.Features.Auth.Dtos;

namespace OnlineTravel.Application.Features.Auth.Login
{
	public class AuthResponse
	{
		public bool IsSuccess { get; set; }
		public string? Message { get; set; }

		public string? Token { get; set; }
		public DateTime? ExpiresAt { get; set; }

		public UserDto? User { get; set; }
	}

}
