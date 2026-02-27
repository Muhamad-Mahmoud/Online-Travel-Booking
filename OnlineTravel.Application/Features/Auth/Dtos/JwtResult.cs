namespace OnlineTravel.Application.Features.Auth.Dtos
{
	public class JwtResult
	{
		public string Token { get; set; } = string.Empty;
		public DateTime ExpiresAt { get; set; }
	}
}
