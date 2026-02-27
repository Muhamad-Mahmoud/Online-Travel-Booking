using System.ComponentModel.DataAnnotations;

namespace OnlineTravel.Application.Features.Auth.Login
{
	public class LoginRequest
	{
		[Required, EmailAddress]
		public required string Email { get; set; }

		[Required, MinLength(6)]
		public required string Password { get; set; }
	}
}
