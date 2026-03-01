using Mapster;
using OnlineTravel.Application.Features.Tours.GetAllTours;
using OnlineTravel.Application.Features.Tours.GetTourById;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.Mapping;

public class TourMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Tour, TourResponse>();
		config.NewConfig<Tour, TourDetailsResponse>();
		config.NewConfig<TourActivity, TourActivityResponse>();
		config.NewConfig<TourImage, TourImageResponse>();
		config.NewConfig<TourPriceTier, TourPriceTierResponse>();
	}
}
