using MediatR;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

namespace OnlineTravel.Application.Features.Tours.GetTourById;

public record GetTourByIdQuery(Guid Id) : IRequest<OnlineTravel.Application.Common.Result<TourDetailsResponse>>;


