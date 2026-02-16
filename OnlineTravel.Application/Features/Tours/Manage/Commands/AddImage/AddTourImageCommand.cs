using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage;

public class AddTourImageCommand : IRequest<Guid>
{
    public Guid TourId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? AltText { get; set; }
}
