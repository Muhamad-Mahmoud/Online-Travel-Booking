namespace OnlineTravel.Api.Errors;

public class ApiExceptionResponse : ApiResponse
{
	public string? Details { get; set; }
	public object? Errors { get; set; }

	public ApiExceptionResponse(int statusCode, string? message = null, string? details = null)
		: base(statusCode, message, false)
	{
		Details = details;
	}
}



