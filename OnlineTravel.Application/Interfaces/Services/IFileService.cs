namespace OnlineTravel.Application.Interfaces.Services;

public interface IFileService
{
	Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder);
	void DeleteFile(string filePath);
	string GetFileUrl(string fileName, string folder);



}
