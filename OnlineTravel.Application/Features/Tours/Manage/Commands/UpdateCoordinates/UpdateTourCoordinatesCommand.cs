using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateCoordinates;

public class UpdateTourCoordinatesCommand : IRequest<OnlineTravel.Domain.ErrorHandling.Result<bool>>
{
	public Guid TourId { get; set; }
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
}

