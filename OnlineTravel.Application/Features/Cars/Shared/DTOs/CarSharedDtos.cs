using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Features.Cars.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineTravel.Application.Features.Cars.Shared.DTOs
{
    public class DateTimeRangeDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class ImageUrlDto
    {
        public string Url { get; set; } = string.Empty;
        public string? AltText { get; set; }
    }

    public class LocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
    }
}
