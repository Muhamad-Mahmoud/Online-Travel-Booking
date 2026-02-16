using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Hotels.Admin.AddRoom;
using OnlineTravel.Application.Features.Hotels.Admin.ConfigureSeasonalPricing;
using OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand;
using OnlineTravel.Application.Features.Hotels.Admin.EditRoom;
using OnlineTravel.Application.Features.Hotels.Admin.ManageAvailability;
using OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravelBookingTeamB.Models;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Roles = "Admin")]

    public class AdminHotelsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;

        public AdminHotelsController(IMediator mediator, IFileService fileService)
        {
            _mediator = mediator;
            _fileService = fileService;
        }

        [HttpPost("hotels")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateHotel([FromForm] CreateHotelFormRequest form)
        {
            string? mainImageUrl = null;
            if (form.MainImage != null && form.MainImage.Length > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(form.MainImage.FileName)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !allowed.Contains(ext))
                    return BadRequest(new { error = "accept : jpg, png, gif, webp." });
                if (form.MainImage.Length > 5 * 1024 * 1024)
                    return BadRequest(new { error = "Not Large 5 " });

                using var stream = form.MainImage.OpenReadStream();
                var relativePath = await _fileService.UploadFileAsync(stream, form.MainImage.FileName, "hotels");
                mainImageUrl = "/" + relativePath.Replace("\\", "/");
            }

            if (!TimeOnly.TryParse(form.CheckInTimeStart, out var checkInStart))
                return BadRequest(new { error = "CheckInTimeStart Not Valid." });
            if (!TimeOnly.TryParse(form.CheckInTimeEnd, out var checkInEnd))
                return BadRequest(new { error = "CheckInTimeEnd Not Valid." });
            if (!TimeOnly.TryParse(form.CheckOutTimeStart, out var checkOutStart))
                return BadRequest(new { error = "CheckOutTimeStart Not Valid." });
            if (!TimeOnly.TryParse(form.CheckOutTimeEnd, out var checkOutEnd))
                return BadRequest(new { error = "CheckOutTimeEnd Not Valid." });

            double? lat = null, lon = null;
            if (!string.IsNullOrWhiteSpace(form.Latitude))
            {
                if (!double.TryParse(form.Latitude, out var parsedLat))
                    return BadRequest(new { error = "Not Valid Latitude" });
                lat = parsedLat;
            }
            if (!string.IsNullOrWhiteSpace(form.Longitude))
            {
                if (!double.TryParse(form.Longitude, out var parsedLon))
                    return BadRequest(new { error = "Not Valid Longitude" });
                lon = parsedLon;
            }

            var command = new CreateHotelCommand
            {
                Name = form.Name,
                Slug = form.Slug,
                Description = form.Description,
                Street = form.Street,
                City = form.City,
                State = form.State ?? "",
                Country = form.Country,
                PostalCode = form.PostalCode ?? "",
                Latitude = lat,
                Longitude = lon,
                CheckInTimeStart = checkInStart,
                CheckInTimeEnd = checkInEnd,
                CheckOutTimeStart = checkOutStart,
                CheckOutTimeEnd = checkOutEnd,
                CancellationPolicy = form.CancellationPolicy,
                ContactPhone = form.ContactPhone,
                ContactEmail = form.ContactEmail,
                Website = form.Website,
                MainImageUrl = mainImageUrl
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            var data = result.Data;
            return data is null ? BadRequest() : CreatedAtRoute("GetHotelDetails", new { id = data.Id }, data);
        }

        [HttpPut("hotels/{id}")]
        public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] UpdateHotelCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }

        [HttpPost("hotels/{hotelId}/rooms")]
        public async Task<IActionResult> AddRoom(Guid hotelId, [FromBody] AddRoomCommand command)
        {
            command.HotelId = hotelId;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return CreatedAtAction(nameof(HotelsController.GetHotelRooms), "Hotels", new { id = hotelId }, result.Data);
        }

        [HttpPut("rooms/{id}")]
        public async Task<IActionResult> EditRoom(Guid id, [FromBody] EditRoomCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }

        [HttpPut("rooms/{id}/availability")]
        public async Task<IActionResult> ManageAvailability(Guid id, [FromBody] ManageAvailabilityCommand command)
        {
            command.RoomId = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }

        [HttpPost("rooms/{id}/seasonal-pricing")]
        public async Task<IActionResult> ConfigureSeasonalPricing(Guid id, [FromBody] ConfigureSeasonalPricingCommand command)
        {
            command.RoomId = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }
    }
}
