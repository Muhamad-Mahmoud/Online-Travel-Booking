using MediatR;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrandById;

public sealed record GetCarBrandByIdQuery(Guid Id) : IRequest<Result<CarBrandDto>>;
