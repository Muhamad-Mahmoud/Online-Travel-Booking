using AutoMapper;
using OnlineTravel.Application.Features.Auth.Dtos;
using OnlineTravel.Application.Features.Auth.Register;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Application.Features.Auth.Mapping;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequest, AppUser>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

        CreateMap<AppUser, UserDto>();
    }
}

