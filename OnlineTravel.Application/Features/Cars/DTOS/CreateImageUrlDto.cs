using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.DTOS
{
    public class CreateImageUrlDto
    {
        public string Url { get; set; }
        public string? AltText { get; set; }
    }
}
