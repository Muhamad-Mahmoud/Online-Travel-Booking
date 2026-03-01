using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

namespace OnlineTravel.Application.Features.Tours.GetTourById;

public record GetTourByIdQuery(Guid Id) : IRequest<Result<TourDetailsResponse>>;


