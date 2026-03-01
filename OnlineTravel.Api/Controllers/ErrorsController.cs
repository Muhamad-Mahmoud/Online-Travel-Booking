using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Api.Errors;

namespace OnlineTravel.Api.Controllers
{
	/// <summary>
	/// Handles application-wide error responses.
	/// </summary>
	[Route("errors/{code}")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		/// <summary>
		/// Catch-all endpoint for returning structured error responses.
		/// </summary>
		public IActionResult Error(int code)
		{
			return new ObjectResult(new ApiResponse(code)) { StatusCode = code };
		}

	}
}


