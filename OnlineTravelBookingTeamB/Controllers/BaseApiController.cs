using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTravelBookingTeamB.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
	protected Guid UserId
	{
		get
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
				throw new UnauthorizedAccessException("User is not authenticated.");

			return Guid.Parse(userId);
		}
	}

}
