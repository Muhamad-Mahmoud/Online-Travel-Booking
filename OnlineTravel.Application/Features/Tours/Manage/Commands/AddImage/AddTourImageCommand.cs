using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage;

public class AddTourImageCommand : IRequest<OnlineTravel.Domain.ErrorHandling.Result<Guid>>
{
	public Guid TourId { get; set; }
	public string Url { get; set; } = string.Empty;
	public string? AltText { get; set; }
}
