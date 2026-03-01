using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.Hotels.Admin.GetHotels;

public record GetHotelsQuery(int PageIndex = 1, int PageSize = 10, string? Search = null)
	: IRequest<Result<PaginatedResult<Hotel>>>;
