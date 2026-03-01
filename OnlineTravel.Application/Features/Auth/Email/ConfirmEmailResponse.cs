namespace OnlineTravel.Application.Features.Auth.Email
{
	public class ConfirmEmailResponse
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; } = null!;
	}

}
