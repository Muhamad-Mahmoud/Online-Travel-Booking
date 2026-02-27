using Mapster;
using OnlineTravel.Application.Features.CarBrands.CreateCarBrand;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Application.Features.CarBrands.UpdateCarBrand;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Mapping
{
	public class CarBrandMappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// Entity -> DTO
			config.NewConfig<CarBrand, CarBrandDto>();

			// Request -> Entity
			config.NewConfig<CreateCarBrandRequest, CarBrand>()
				.Ignore(dest => dest.Id)
				.Ignore(dest => dest.CreatedAt)
				.Ignore(dest => dest.UpdatedAt)
				.Ignore(dest => dest.Cars);

			config.NewConfig<UpdateCarBrandRequest, CarBrand>()
				.Ignore(dest => dest.CreatedAt)
				.Ignore(dest => dest.UpdatedAt)
				.Ignore(dest => dest.Cars);
		}
	}
}
