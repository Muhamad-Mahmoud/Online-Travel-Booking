using Mapster;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Mapping
{
    public class CarBrandMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<CarBrand, CarBrandDto>();

            // Create DTO -> Entity (ignore Id, CreatedAt, UpdatedAt)
            config.NewConfig<CreateCarBrandDto, CarBrand>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.Cars);   // navigation property

            config.NewConfig<UpdateCarBrandDto, CarBrand>()
           .Ignore(dest => dest.Id)          // احتياطي، لو حصل وأضيف Id في المستقبل
           .Ignore(dest => dest.CreatedAt)
           .Ignore(dest => dest.UpdatedAt)
           .Ignore(dest => dest.Cars);
        }
    }
}
