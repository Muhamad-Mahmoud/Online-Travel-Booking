using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Infrastructure.Persistence.Context;
using MediatR;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravelBookingTeamB.Models;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier;
using OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateTour;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddActivity;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage;
using OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateCoordinates;
using OnlineTravel.Application.Features.Tours.GetAllTours.Queries;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;
using OnlineTravel.Application.Features.Tours.CreateTour.Commands;
using OnlineTravel.Application.Features.Tours.DeleteTour.Commands;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin/Tours")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ToursController : Controller
    {
        private readonly OnlineTravelDbContext _context;
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;

        public ToursController(OnlineTravelDbContext context, IMediator mediator, IFileService fileService)
        {
            _context = context;
            _mediator = mediator;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 20)
        {
            var query = new GetAllToursQuery(pageIndex, pageSize, null, null, null, null, null, null, null, null, null, null);
            var result = await _mediator.Send(query);

            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Tour)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View("~/Views/Admin/Tours/Tours/Index.cshtml", result.Data);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Tour)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
            return View("~/Views/Admin/Tours/Tours/Create.cshtml");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(TourViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    string mainImageUrl = string.Empty;
                    if (model.MainImage != null)
                    {
                        using (var stream = model.MainImage.OpenReadStream())
                        {
                            mainImageUrl = await _fileService.UploadFileAsync(stream, model.MainImage.FileName, "tours");
                        }
                    }

                    var command = new CreateTourCommand
                    {
                        Title = model.Title,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        DurationDays = model.DurationDays,
                        DurationNights = model.DurationNights,
                        Recommended = model.Recommended,
                        BestTimeToVisit = model.BestTimeToVisit,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                        PostalCode = model.PostalCode,
                        MainImageUrl = mainImageUrl,
                        StandardPrice = model.StandardPrice,
                        Currency = model.Currency
                    };

                    await _mediator.Send(command);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating tour: " + ex.Message);
                }
            }
            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Tour)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
            return View("~/Views/Admin/Tours/Tours/Create.cshtml", model);
        }

        [HttpGet("Manage/{id}")]
        public async Task<IActionResult> Manage(Guid id, bool partial = false)
        {
            var query = new GetTourByIdQuery(id);
            var tour = await _mediator.Send(query);

            if (tour == null)
            {
                return NotFound();
            }

            var model = new ManageTourViewModel
            {
                Tour = tour,
                EditForm = new EditTourViewModel
                {
                    TourId = id,
                    Title = tour.Title,
                    Description = tour.Description,
                    CategoryId = tour.CategoryId,
                    DurationDays = tour.DurationDays,
                    DurationNights = tour.DurationNights,
                    BestTimeToVisit = tour.BestTimeToVisit,
                    Recommended = tour.Recommended,
                    Street = tour.Location?.Street,
                    City = tour.Location?.City,
                    State = tour.Location?.State,
                    Country = tour.Location?.Country,
                    PostalCode = tour.Location?.PostalCode
                },
                ActivityForm = new AddActivityViewModel { TourId = id },
                ImageForm = new AddTourImageViewModel { TourId = id },
                PriceTierForm = new AddTourPriceTierCommand { TourId = id },
                LocationForm = new UpdateCoordinatesViewModel 
                { 
                    TourId = id, 
                    Latitude = tour.Location?.Coordinates?.Y, 
                    Longitude = tour.Location?.Coordinates?.X 
                }
            };

            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Tour)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title", tour.CategoryId);
            if (partial)
                return PartialView("~/Views/Admin/Tours/Tours/Manage.cshtml", model);

            return View("~/Views/Admin/Tours/Tours/Manage.cshtml", model);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(EditTourViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string? mainImageUrl = null;
                    if (model.MainImage != null)
                    {
                        using (var stream = model.MainImage.OpenReadStream())
                        {
                            mainImageUrl = await _fileService.UploadFileAsync(stream, model.MainImage.FileName, "tours");
                        }
                    }

                    var command = new UpdateTourCommand
                    {
                        TourId = model.TourId,
                        Title = model.Title,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        DurationDays = model.DurationDays,
                        DurationNights = model.DurationNights,
                        Recommended = model.Recommended,
                        BestTimeToVisit = model.BestTimeToVisit,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                        PostalCode = model.PostalCode,
                        MainImageUrl = mainImageUrl
                    };

                    await _mediator.Send(command);
                    TempData["Success"] = "Tour updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating tour: " + ex.Message);
                }
            }
            return RedirectToAction(nameof(Manage), new { id = model.TourId });
        }


        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var command = new DeleteTourCommand(id);
                await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting tour: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("AddActivity")]
        public async Task<IActionResult> AddActivity([Bind(Prefix = "ActivityForm")] AddActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = string.Empty;
                if (model.Image != null)
                {
                    using (var stream = model.Image.OpenReadStream())
                    {
                        imageUrl = await _fileService.UploadFileAsync(stream, model.Image.FileName, "tours/activities");
                    }
                }

                var command = new AddTourActivityCommand
                {
                    TourId = model.TourId,
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = imageUrl
                };

                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(Manage), new { id = model.TourId });
        }

        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage([Bind(Prefix = "ImageForm")] AddTourImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = string.Empty;
                if (model.Image != null)
                {
                    using (var stream = model.Image.OpenReadStream())
                    {
                        imageUrl = await _fileService.UploadFileAsync(stream, model.Image.FileName, "tours/gallery");
                    }
                }

                var command = new AddTourImageCommand
                {
                    TourId = model.TourId,
                    Url = imageUrl,
                    AltText = model.AltText
                };

                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(Manage), new { id = model.TourId });
        }

        [HttpPost("AddPriceTier")]
        public async Task<IActionResult> AddPriceTier([Bind(Prefix = "PriceTierForm")] AddTourPriceTierCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(Manage), new { id = command.TourId });
        }

        [HttpPost("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation(UpdateCoordinatesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateTourCoordinatesCommand
                {
                    TourId = model.TourId,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };
                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(Manage), new { id = model.TourId });
        }
    }
}
