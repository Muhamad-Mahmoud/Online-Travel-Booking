using Microsoft.AspNetCore.Http;
using OnlineTravel.Application.Features.Cars.CreateCar;
using OnlineTravel.Application.Features.Cars.UpdateCar;

namespace OnlineTravel.Mvc.Models;

public class CarCreateViewModel : CreateCarRequest
{
    public List<IFormFile>? ImageFiles { get; set; }
}

public class CarEditViewModel : UpdateCarRequest
{
    public List<IFormFile>? ImageFiles { get; set; }
}
