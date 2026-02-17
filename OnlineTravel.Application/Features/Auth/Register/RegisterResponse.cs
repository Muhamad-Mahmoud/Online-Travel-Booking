using OnlineTravel.Application.Features.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Auth.Register
{
    public class RegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }

        public string? ConfirmationLink { get; set; }

        public UserDto? User { get; set; }
    }

}
