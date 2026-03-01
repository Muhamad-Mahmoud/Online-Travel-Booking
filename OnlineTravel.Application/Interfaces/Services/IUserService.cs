namespace OnlineTravel.Application.Interfaces.Services;

public interface IUserService
{
	Task<bool> UserExistsAsync(Guid userId);
}
