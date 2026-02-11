using MediatR;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

namespace OnlineTravel.Application.Features.Tours.GetTourById.Queries;

public record GetTourByIdQuery(Guid Id) : IRequest<TourDetailsResponse?>;
