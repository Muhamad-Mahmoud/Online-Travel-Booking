using Mapster;
using OnlineTravel.Application.Features.Auth.Register;
using OnlineTravel.Application.Features.Auth.Shared;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Application.Features.Auth.Mapping;

public class AuthMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<RegisterRequest, AppUser>()
			.Map(d => d.UserName, s => s.Email);

		config.NewConfig<AppUser, UserResponse>();
	}
}
