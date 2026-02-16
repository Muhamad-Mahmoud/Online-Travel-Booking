using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.DTOs
{
    public class ImageUrlDto
    {
        public string Url { get; set; } = string.Empty;
        public string? AltText { get; set; }
    }
}
