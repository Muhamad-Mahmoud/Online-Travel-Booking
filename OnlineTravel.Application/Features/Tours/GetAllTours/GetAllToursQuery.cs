using MediatR;

namespace OnlineTravel.Application.Features.Tours.GetAllTours;

using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

public record GetAllToursQuery(int PageIndex, int PageSize, string? Search, double? Lat, double? Lon, double? RadiusKm, decimal? MinPrice, decimal? MaxPrice, int? Rating, string? City, string? Country, string? SortOrder) : IRequest<Result<PagedResult<TourResponse>>>;

