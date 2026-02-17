using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Auth.Password
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = default!;
    }
}
