using MediatR;
using OnlineTravel.Application.Features.Tours.GetAllTours.DTOs;

namespace OnlineTravel.Application.Features.Tours.GetAllTours.Queries;

using OnlineTravel.Application.Common;

public record GetAllToursQuery(int PageIndex, int PageSize) : IRequest<PagedResult<TourResponse>>;
