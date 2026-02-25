using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Infrastructure.Persistence.Context;
using MediatR;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var query = new OnlineTravel.Application.Features.Admin.Dashboard.GetAdminDashboardStatsQuery();
            var result = await _mediator.Send(query);
            return View("~/Views/Admin/Index.cshtml", result.Value);
        }

        [HttpGet]
        [Route("SeedData")]
        public IActionResult SeedData()
        {
            return View("~/Views/Admin/System/SeedData.cshtml");
        }

        [HttpPost]
        [Route("SeedData/Trigger")]
        public async Task<IActionResult> TriggerSeed()
        {
            using var scope = HttpContext.RequestServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<OnlineTravelDbContext>();
            var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<OnlineTravel.Domain.Entities.Users.AppUser>>();
            var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole<Guid>>>();

            await OnlineTravel.Infrastructure.Persistence.Seed.ApplicationDbContextSeed.SeedAsync(context, userManager, roleManager);

            TempData["Success"] = "Database seeded successfully with unified data!";
            return RedirectToAction("SeedData");
        }
    }
}
