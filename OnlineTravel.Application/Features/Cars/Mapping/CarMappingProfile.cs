using AutoMapper;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Features.Cars.Shared.DTOs;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Features.Cars.Mapping
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile()
        {
            CreateMap<CarBrand, OnlineTravel.Application.Features.CarBrands.Shared.DTOs.CarBrandDto>();
            
            CreateMap<Car, CarDto>()
                .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(s => s.Category.Title))
                .ForMember(d => d.Location, opt => opt.MapFrom(s => s.Location));

            CreateMap<DateTimeRange, DateTimeRangeDto>();
            CreateMap<ImageUrl, ImageUrlDto>()
                .ForMember(d => d.Url, opt => opt.MapFrom(s => s.Url));
            CreateMap<NetTopologySuite.Geometries.Point, LocationDto>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Y))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.X));
        }
    }
}
