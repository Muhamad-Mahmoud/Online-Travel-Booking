using MediatR;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Domain.ErrorHandling;
using System.Collections.Generic;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrands;

public sealed record GetCarBrandsQuery() : IRequest<Result<List<CarBrandDto>>>;
