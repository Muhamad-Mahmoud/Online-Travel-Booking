using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Interfaces.Services;

namespace OnlineTravel.Api.Controllers
{
	[Route("api/v1/[controller]")]
	[Authorize(Roles = "Admin")]
	public class UploadController : BaseApiController
	{
		public sealed class UploadImageRequest
		{
			public IFormFile File { get; set; } = default!;
		}

		private const long MaxFileSizeBytes = 5 * 1024 * 1024;
		private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

		private readonly IFileService _fileService;
		private readonly ILogger<UploadController> _logger;

		public UploadController(IFileService fileService, ILogger<UploadController> logger)
		{
			_fileService = fileService;
			_logger = logger;
		}

		[HttpPost("hotel-image")]
		[Consumes("multipart/form-data")]
		[RequestSizeLimit(MaxFileSizeBytes)]
		public async Task<IActionResult> UploadHotelImage([FromForm] UploadImageRequest request)
		{
			var file = request.File;
			if (!TryValidateImageFile(file, out var error))
				return BadRequest(error);

			try
			{
				using var stream = file.OpenReadStream();
				var url = await _fileService.UploadFileAsync(stream, file.FileName, "hotels");
				return Ok(new { url, message = "File uploaded successfully" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to upload hotel image.");
				return Problem("Failed to upload image.");
			}
		}

		[HttpPost("room-image")]
		[Consumes("multipart/form-data")]
		[RequestSizeLimit(MaxFileSizeBytes)]
		public async Task<IActionResult> UploadRoomImage([FromForm] UploadImageRequest request)
		{
			var file = request.File;
			if (!TryValidateImageFile(file, out var error))
				return BadRequest(error);

			try
			{
				using var stream = file.OpenReadStream();
				var url = await _fileService.UploadFileAsync(stream, file.FileName, "rooms");
				return Ok(new { url, message = "File uploaded successfully" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to upload room image.");
				return Problem("Failed to upload image.");
			}
		}

		private static bool TryValidateImageFile(IFormFile file, out string? error)
		{
			if (file == null || file.Length == 0)
			{
				error = "Invalid file.";
				return false;
			}

			if (file.Length > MaxFileSizeBytes)
			{
				error = "File must not exceed 5 MB.";
				return false;
			}

			var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
			if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
			{
				error = "Accepted formats: jpg, jpeg, png, gif, webp.";
				return false;
			}

			error = null;
			return true;
		}

	}

}

