
using OnlineTravel.Application.Features.Auth.Shared;

namespace OnlineTravel.Application.Features.Auth.Register
{
	public class RegisterResponse
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; } = string.Empty;

		public bool EmailConfirmed { get; set; }

		public string? ConfirmationLink { get; set; }

		public UserResponse? User { get; set; }
	}

}
