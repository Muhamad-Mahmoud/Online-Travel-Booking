using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTravel.Mvc.Controllers;

public abstract class BaseController : Controller
{
	private IMediator? _mediator;
	protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
