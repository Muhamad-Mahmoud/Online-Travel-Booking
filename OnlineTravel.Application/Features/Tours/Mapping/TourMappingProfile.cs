using AutoMapper;
using OnlineTravel.Application.Features.Tours.GetAllTours;
using OnlineTravel.Application.Features.Tours.GetTourById;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.Mapping
{
	public class TourMappingProfile : Profile
	{
		public TourMappingProfile()
		{
			CreateMap<Tour, TourResponse>();
			CreateMap<Tour, TourDetailsResponse>();
			CreateMap<TourActivity, TourActivityResponse>();
			CreateMap<TourImage, TourImageResponse>();
			CreateMap<TourPriceTier, TourPriceTierResponse>();
		}
	}
}
