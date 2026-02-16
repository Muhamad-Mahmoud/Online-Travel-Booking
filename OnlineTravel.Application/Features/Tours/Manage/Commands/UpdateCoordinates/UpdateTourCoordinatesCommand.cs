using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateCoordinates;

public class UpdateTourCoordinatesCommand : IRequest<bool>
{
    public Guid TourId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
