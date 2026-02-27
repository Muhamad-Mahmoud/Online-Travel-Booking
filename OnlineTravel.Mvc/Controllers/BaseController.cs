using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTravel.Mvc.Controllers;

[Authorize(Roles = "Admin")]
public abstract class BaseController : Controller
{
	private IMediator? _mediator;
	protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
