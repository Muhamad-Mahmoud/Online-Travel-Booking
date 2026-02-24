using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Infrastructure.Persistence.Context;
using MediatR;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand;
using OnlineTravel.Application.Features.Hotels.Admin.AddRoom;
using OnlineTravel.Application.Features.Hotels.Admin.EditRoom;
using OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel;
using OnlineTravelBookingTeamB.Models;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin/Hotels")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HotelsController : Controller
    {
        private readonly OnlineTravelDbContext _context;
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;

        public HotelsController(OnlineTravelDbContext context, IMediator mediator, IFileService fileService)
        {
            _context = context;
            _mediator = mediator;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(string? search = null)
        {
            var query = _context.Hotels
                .Include(h => h.Rooms)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(h => h.Name.ToLower().Contains(term) || h.Slug.ToLower().Contains(term));
            }

            var hotels = await query.OrderByDescending(h => h.CreatedAt).ToListAsync();
            ViewBag.SearchTerm = search;
            return View("~/Views/Admin/Hotels/Hotels/Index.cshtml", hotels);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Hotels/Hotels/Create.cshtml", new CreateHotelViewModel());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateHotelViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Hotels/Hotels/Create.cshtml", model);

            try
            {
                string? mainImageUrl = null;
                if (model.MainImage != null && model.MainImage.Length > 0)
                {
                    using var stream = model.MainImage.OpenReadStream();
                    var relativePath = await _fileService.UploadFileAsync(stream, model.MainImage.FileName, "hotels");
                    mainImageUrl = "/" + relativePath.Replace("\\", "/");
                }

                if (!TimeOnly.TryParse(model.CheckInTimeStart, out var checkInStart)) checkInStart = new TimeOnly(14, 0);
                if (!TimeOnly.TryParse(model.CheckInTimeEnd, out var checkInEnd)) checkInEnd = new TimeOnly(16, 0);
                if (!TimeOnly.TryParse(model.CheckOutTimeStart, out var checkOutStart)) checkOutStart = new TimeOnly(10, 0);
                if (!TimeOnly.TryParse(model.CheckOutTimeEnd, out var checkOutEnd)) checkOutEnd = new TimeOnly(12, 0);

                double? lat = null, lon = null;
                if (double.TryParse(model.Latitude, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsedLat)) lat = parsedLat;
                if (double.TryParse(model.Longitude, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsedLon)) lon = parsedLon;

                var command = new CreateHotelCommand
                {
                    Name = model.Name,
                    Slug = model.Slug,
                    Description = model.Description,
                    Street = model.Street,
                    City = model.City,
                    State = model.State ?? "",
                    Country = model.Country,
                    PostalCode = model.PostalCode ?? "",
                    Latitude = lat,
                    Longitude = lon,
                    CheckInTimeStart = checkInStart,
                    CheckInTimeEnd = checkInEnd,
                    CheckOutTimeStart = checkOutStart,
                    CheckOutTimeEnd = checkOutEnd,
                    CancellationPolicy = model.CancellationPolicy,
                    ContactPhone = model.ContactPhone,
                    ContactEmail = model.ContactEmail,
                    Website = model.Website,
                    MainImageUrl = mainImageUrl
                };

                var result = await _mediator.Send(command);
                if (!result.IsSuccess)
                {
                    ModelState.AddModelError("", result.ErrorMessage ?? "Failed to create hotel.");
                    return View("~/Views/Admin/Hotels/Hotels/Create.cshtml", model);
                }

                TempData["Success"] = "Hotel created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating hotel: " + ex.Message);
                return View("~/Views/Admin/Hotels/Hotels/Create.cshtml", model);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var hotel = await _context.Hotels.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null) return NotFound();

            var model = new Models.EditHotelViewModel
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                Street = hotel.Address?.Street ?? "",
                City = hotel.Address?.City ?? "",
                State = hotel.Address?.State,
                Country = hotel.Address?.Country ?? "",
                PostalCode = hotel.Address?.PostalCode,
                Latitude = hotel.Address?.Coordinates?.Y ?? 0,
                Longitude = hotel.Address?.Coordinates?.X ?? 0,
                CancellationPolicy = hotel.CancellationPolicy ?? "",
                ContactPhone = hotel.ContactInfo?.Phone?.Value,
                ContactEmail = hotel.ContactInfo?.Email?.Value,
                Website = hotel.ContactInfo?.Website?.ToString(),
                CurrentImageUrl = hotel.MainImageUrl
            };

            return View("~/Views/Admin/Hotels/Hotels/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(EditHotelViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Hotels/Hotels/Edit.cshtml", model);

            try
            {
                if (!TimeOnly.TryParse(model.CheckInTime, out var checkIn)) checkIn = new TimeOnly(14, 0);
                if (!TimeOnly.TryParse(model.CheckOutTime, out var checkOut)) checkOut = new TimeOnly(10, 0);

                var command = new UpdateHotelCommand
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Street = model.Street,
                    City = model.City,
                    State = model.State ?? "",
                    Country = model.Country,
                    PostalCode = model.PostalCode ?? "",
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    CheckInTime = checkIn,
                    CheckOutTime = checkOut,
                    CancellationPolicy = model.CancellationPolicy,
                    ContactPhone = model.ContactPhone ?? "",
                    ContactEmail = model.ContactEmail ?? "",
                    Website = model.Website ?? "",
                    MainImage = model.MainImage ?? model.CurrentImageUrl ?? ""
                };

                var result = await _mediator.Send(command);
                if (!result.IsSuccess)
                {
                    ModelState.AddModelError("", result.ErrorMessage ?? "Failed to update hotel.");
                    return View("~/Views/Admin/Hotels/Hotels/Edit.cshtml", model);
                }

                TempData["Success"] = "Hotel updated successfully!";
                return RedirectToAction(nameof(Manage), new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating hotel: " + ex.Message);
                return View("~/Views/Admin/Hotels/Hotels/Edit.cshtml", model);
            }
        }

        [HttpGet("Manage/{id}")]
        public async Task<IActionResult> Manage(Guid id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null) return NotFound();

            var model = new ManageHotelViewModel
            {
                Hotel = hotel,
                EditForm = new EditHotelViewModel
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Description = hotel.Description,
                    Street = hotel.Address?.Street ?? "",
                    City = hotel.Address?.City ?? "",
                    State = hotel.Address?.State,
                    Country = hotel.Address?.Country ?? "",
                    PostalCode = hotel.Address?.PostalCode,
                    Latitude = hotel.Address?.Coordinates?.Y ?? 0,
                    Longitude = hotel.Address?.Coordinates?.X ?? 0,
                    CancellationPolicy = hotel.CancellationPolicy ?? "",
                    ContactPhone = hotel.ContactInfo?.Phone?.Value,
                    ContactEmail = hotel.ContactInfo?.Email?.Value,
                    Website = hotel.ContactInfo?.Website?.ToString(),
                    CurrentImageUrl = hotel.MainImageUrl
                },
                RoomForm = new AddRoomViewModel { HotelId = id }
            };

            return View("~/Views/Admin/Hotels/Hotels/Manage.cshtml", model);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return NotFound();

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Hotel deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("AddRoom")]
        public async Task<IActionResult> AddRoom(AddRoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Manage), new { id = model.HotelId });
            }

            try
            {
                var command = new AddRoomCommand
                {
                    HotelId = model.HotelId,
                    RoomNumber = model.RoomNumber,
                    Name = model.Name,
                    Description = model.Description,
                    BasePricePerNight = model.BasePricePerNight
                };

                var result = await _mediator.Send(command);
                if (!result.IsSuccess)
                {
                    TempData["Error"] = result.ErrorMessage ?? "Failed to add room.";
                }
                else
                {
                    TempData["Success"] = $"Room '{model.Name}' added successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error adding room: " + ex.Message;
            }

            return RedirectToAction(nameof(Manage), new { id = model.HotelId });
        }

        [HttpPost("EditRoom")]
        public async Task<IActionResult> EditRoom(EditRoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid room data.";
                return RedirectToAction(nameof(Manage), new { id = model.HotelId });
            }

            try
            {
                var command = new EditRoomCommand
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    BasePricePerNight = model.BasePricePerNight
                };

                var result = await _mediator.Send(command);
                if (!result.IsSuccess)
                    TempData["Error"] = result.ErrorMessage ?? "Failed to update room.";
                else
                    TempData["Success"] = $"Room '{model.Name}' updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating room: " + ex.Message;
            }

            return RedirectToAction(nameof(Manage), new { id = model.HotelId });
        }

        [HttpPost("DeleteRoom")]
        public async Task<IActionResult> DeleteRoom(Guid id, Guid hotelId)
        {
            var room = await _context.Set<OnlineTravel.Domain.Entities.Hotels.Room>().FindAsync(id);
            if (room != null)
            {
                _context.Set<OnlineTravel.Domain.Entities.Hotels.Room>().Remove(room);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Room deleted successfully.";
            }
            return RedirectToAction(nameof(Manage), new { id = hotelId });
        }
    }
}
