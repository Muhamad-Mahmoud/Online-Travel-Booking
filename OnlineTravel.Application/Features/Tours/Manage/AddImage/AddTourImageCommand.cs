using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.AddImage;

public class AddTourImageCommand : IRequest<Domain.ErrorHandling.Result<Guid>>
{
	public Guid TourId { get; set; }
	public string Url { get; set; } = string.Empty;
	public string? AltText { get; set; }
}
