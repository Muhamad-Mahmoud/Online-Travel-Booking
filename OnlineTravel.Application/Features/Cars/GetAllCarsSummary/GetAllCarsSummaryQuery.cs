using MediatR;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Cars.Shared;

namespace OnlineTravel.Application.Features.Cars.GetAllCarsSummary;

public sealed record GetAllCarsSummaryQuery(
	int PageNumber = 1,
	int PageSize = 5,
	Guid? BrandId = null,
	Guid? CategoryId = null,
	CarCategory? CarType = null,
	string? SearchTerm = null
) : IRequest<Result<PagedResult<CarSummaryResponse>>>;


