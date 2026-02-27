using OnlineTravel.Application.Interfaces.Services;

namespace OnlineTravel.Infrastructure.Services;

public class FileService : IFileService
{
	private readonly string _basePath;

	public FileService(string basePath)
	{
		_basePath = basePath;
	}

	public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder)
	{
		var folderPath = Path.Combine(_basePath, "uploads", folder);
		Directory.CreateDirectory(folderPath);

		var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
		var filePath = Path.Combine(folderPath, uniqueFileName);

		using var stream = new FileStream(filePath, FileMode.Create);
		await fileStream.CopyToAsync(stream);

		// Return relative path for storage
		return Path.Combine("uploads", folder, uniqueFileName).Replace("\\", "/");
	}

	public void DeleteFile(string filePath)
	{
		var fullPath = Path.Combine(_basePath, filePath);
		if (File.Exists(fullPath))
			File.Delete(fullPath);
	}

	public string GetFileUrl(string fileName, string folder)
	{
		return $"/uploads/{folder}/{fileName}";
	}
}
