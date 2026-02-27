using MediatR;

namespace OnlineTravel.Application.Features.Tours.DeleteTour.Commands
{
	public class DeleteTourCommand : IRequest<OnlineTravel.Domain.ErrorHandling.Result<bool>>
	{
		public Guid Id { get; set; }

		public DeleteTourCommand(Guid id)
		{
			Id = id;
		}
	}
}

