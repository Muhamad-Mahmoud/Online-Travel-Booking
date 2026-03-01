using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Infrastructure.Security;

public class UserService : IUserService
{
	private readonly UserManager<AppUser> _userManager;

	public UserService(UserManager<AppUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<bool> UserExistsAsync(Guid userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		return user != null;
	}
}
