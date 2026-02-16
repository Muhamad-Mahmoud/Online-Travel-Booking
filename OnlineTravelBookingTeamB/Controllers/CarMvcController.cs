
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.Queries;
using OnlineTravelBookingTeamB.Models;

namespace OnlineTravelBookingTeamB.Controllers
{
    public class CarMvcController : Controller
    {
        private readonly IMediator _mediator;

        public CarMvcController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /CarMvc
        public async Task<IActionResult> Index(
          int pageIndex = 1,
          int pageSize = 12,
          Guid? brandId = null,
          Guid? categoryId = null,
          OnlineTravel.Domain.Enums.CarCategory? selectedCarType = null)
        {
            // بناء الـ query بالفلترات اللي المستخدم اختارها
            var query = new GetAllCarsSummaryQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                BrandId = brandId,
                CategoryId = categoryId,
                CarType = selectedCarType
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                // ممكن تعرض صفحة خطأ
                return View("Error", result.Error);
            }

            // تجهيز ViewModel يضم النتائج والفلترات عشان الفلتر يحتفظ بحالته
            var viewModel = new CarIndexViewModel
            {
                Cars = result.Value,
                BrandId = brandId,
                CategoryId = categoryId,
                CarType = selectedCarType,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return View("~/Views/Admin/CarMvc/Index.cshtml", viewModel);
        }
    }
}
