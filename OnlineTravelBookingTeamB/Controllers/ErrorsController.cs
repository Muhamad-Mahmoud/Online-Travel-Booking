using OnlineTravelBookingTeamB.Errors;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTravelBookingTeamB.Controllers
{
    /// <summary>
    /// Handles application-wide error responses.
    /// </summary>
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code)) { StatusCode = code };
        }

    }
}

