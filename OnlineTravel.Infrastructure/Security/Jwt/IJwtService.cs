using OnlineTravel.Application.Features.Auth.Shared;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Infrastructure.Security.Jwt
{
	public interface IJwtService
	{
		Task<JwtResult> GenerateToken(AppUser user);
	}
}
