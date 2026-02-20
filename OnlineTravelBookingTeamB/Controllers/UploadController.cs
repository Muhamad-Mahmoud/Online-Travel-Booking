using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Interfaces.Services;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IFileService _fileService;

        public UploadController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost("hotel-image")]
        public async Task<IActionResult> UploadHotelImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("File must not exceed 5 MB.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext))
                return BadRequest("Accepted formats: jpg, jpeg, png, gif, webp.");

            try
            {
                using var stream = file.OpenReadStream();
                var url = await _fileService.UploadFileAsync(stream, file.FileName, "hotels");
                return Ok(new { url, message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("room-image")]
        public async Task<IActionResult> UploadRoomImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("File must not exceed 5 MB.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext))
                return BadRequest("Accepted formats: jpg, jpeg, png, gif, webp.");

            try
            {
                using var stream = file.OpenReadStream();
                var url = await _fileService.UploadFileAsync(stream, file.FileName, "rooms");
                return Ok(new { url, message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }

}
