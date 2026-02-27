using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.AddActivity;

public class AddTourActivityCommand : IRequest<Domain.ErrorHandling.Result<Guid>>
{
	public Guid TourId { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
}
