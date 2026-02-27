namespace OnlineTravel.Application.Features.Auth.Email
{
	public class EmailSettings
	{
		public string From { get; set; } = null!;
		public string SmtpServer { get; set; } = null!;
		public int Port { get; set; }
		public string Username { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
