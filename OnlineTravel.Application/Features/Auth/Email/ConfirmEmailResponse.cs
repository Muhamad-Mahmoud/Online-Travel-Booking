using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Auth.ConfirmEmail
{
    public class ConfirmEmailResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

}
