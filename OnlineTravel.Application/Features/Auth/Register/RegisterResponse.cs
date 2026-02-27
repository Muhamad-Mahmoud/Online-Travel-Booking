using OnlineTravel.Application.Features.Auth.Dtos;

namespace OnlineTravel.Application.Features.Auth.Register
{
	public class RegisterResponse
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; } = string.Empty;

		public bool EmailConfirmed { get; set; }

		public string? ConfirmationLink { get; set; }

		public UserDto? User { get; set; }
	}

}
