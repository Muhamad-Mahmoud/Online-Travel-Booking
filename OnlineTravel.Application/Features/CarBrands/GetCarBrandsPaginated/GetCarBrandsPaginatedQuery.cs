using MediatR;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated;

public sealed record GetCarBrandsPaginatedQuery(
    int PageIndex,
    int PageSize,
    string? SearchTerm = null
) : IRequest<Result<PaginatedResult<CarBrandDto>>>;
