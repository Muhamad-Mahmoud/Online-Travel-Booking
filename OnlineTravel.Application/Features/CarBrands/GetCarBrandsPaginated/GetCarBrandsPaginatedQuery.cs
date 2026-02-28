using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.CarBrands.Shared;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated;

public sealed record GetCarBrandsPaginatedQuery(
	int PageIndex,
	int PageSize,
	string? SearchTerm = null
) : IRequest<Result<PagedResult<CarBrandResponse>>>;

