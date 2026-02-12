using OnlineTravel.Application.Features.Auth.Dtos;
using OnlineTravel.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Infrastructure.Security.Jwt
{
    public interface IJwtService
    {
        Task<JwtResult> GenerateToken(AppUser user);
    }
}
