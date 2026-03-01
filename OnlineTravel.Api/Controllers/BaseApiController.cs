using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Api.Extensions;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Api.Controllers;

[ApiController]
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

			if (!Guid.TryParse(userId, out var parsedUserId))
				throw new UnauthorizedAccessException("Invalid user id claim.");

			return parsedUserId;
		}
	}

	protected ActionResult HandleResult(Result result) => result.ToResponse();
	protected ActionResult HandleResult<T>(Result<T> result) => result.ToResponse();

}

