using MediatR;
using OnlineTravel.Application.Features.Tours.GetAllTours.DTOs;

namespace OnlineTravel.Application.Features.Tours.GetAllTours.Queries;

public record GetAllToursQuery() : IRequest<IReadOnlyList<TourResponse>>;
