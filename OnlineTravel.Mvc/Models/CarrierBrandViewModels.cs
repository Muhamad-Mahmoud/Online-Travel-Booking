using Microsoft.AspNetCore.Http;
using OnlineTravel.Application.Features.Flights.Carrier.CreateCarrier;
using OnlineTravel.Application.Features.CarBrands.CreateCarBrand;
using OnlineTravel.Application.Features.CarBrands.UpdateCarBrand;

namespace OnlineTravel.Mvc.Models;

public class CarrierCreateViewModel : CreateCarrierCommand
{
    public IFormFile? LogoFile { get; set; }
}

public class BrandCreateViewModel : CreateCarBrandRequest
{
    public IFormFile? LogoFile { get; set; }
}

public class BrandEditViewModel : UpdateCarBrandRequest
{
    public IFormFile? LogoFile { get; set; }
}
