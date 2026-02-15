using MediatR;
using OnlineTravel.Application.Features.Tours.GetAllTours.DTOs;

namespace OnlineTravel.Application.Features.Tours.GetAllTours.Queries;

using OnlineTravel.Application.Common;

public record GetAllToursQuery(int PageIndex, int PageSize, string? Search, double? Lat, double? Lon, double? RadiusKm, decimal? MinPrice, decimal? MaxPrice, int? Rating, string? City, string? Country, string? SortOrder) : IRequest<PagedResult<TourResponse>>;
