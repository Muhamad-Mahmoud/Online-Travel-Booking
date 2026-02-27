using MediatR;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.Cars.GetAllCarsSummary;

public sealed record GetAllCarsSummaryQuery(
	int PageIndex = 1,
	int PageSize = 5,
	Guid? BrandId = null,
	Guid? CategoryId = null,
	CarCategory? CarType = null,
	string? SearchTerm = null
) : IRequest<Result<PaginatedResult<CarSummaryDto>>>;
