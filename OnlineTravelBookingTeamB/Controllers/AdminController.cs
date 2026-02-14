using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTravelBookingTeamB.Controllers
{
    // [Authorize(Roles = "Admin")] // Uncomment when roles are ready
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SeedData()
        {
            return View();
        }

        public IActionResult Monitor()
        {
            return View();
        }

        public IActionResult Tours()
        {
            return View();
        }

        public IActionResult Hotels()
        {
            return View();
        }

        public IActionResult Cars()
        {
            return View();
        }

        public IActionResult Flights()
        {
            return View();
        }
    }
}
