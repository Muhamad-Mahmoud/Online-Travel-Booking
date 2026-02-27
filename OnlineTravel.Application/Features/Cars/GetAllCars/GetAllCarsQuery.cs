using MediatR;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.Cars.GetAllCars;

public sealed record GetAllCarsQuery(
	int PageIndex = 1,
	int PageSize = 10,
	Guid? BrandId = null,
	Guid? CategoryId = null,
	CarCategory? CarType = null
) : IRequest<Result<PaginatedResult<CarDto>>>;
