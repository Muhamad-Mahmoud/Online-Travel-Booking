using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Admin.System.TriggerSeed;

namespace OnlineTravel.Mvc.Controllers;

[Route("Admin/System")]
public class SystemController : BaseController
{
	[HttpGet("SeedData")]
	public IActionResult SeedData()
	{
		return View();
	}

	[HttpPost("TriggerSeed")]
	public async Task<IActionResult> TriggerSeed()
	{
		var result = await Mediator.Send(new TriggerSeedCommand());
		if (result.IsSuccess)
		{
			TempData["Success"] = "Database Seeding Completed Successfully!";
		}
		else
		{
			TempData["Error"] = result.Error;
		}
		return RedirectToAction(nameof(SeedData));
	}
}

