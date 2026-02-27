namespace OnlineTravel.Application.Features.Auth.Login
{
	public class RestoreAccountRequest
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
