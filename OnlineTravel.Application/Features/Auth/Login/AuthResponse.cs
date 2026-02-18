using OnlineTravel.Application.Features.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Auth.Login
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public UserDto? User { get; set; }
    }

}
