using MediatR;

namespace OnlineTravel.Application.Features.Tours.DeleteTour
{
	public class DeleteTourCommand : IRequest<Domain.ErrorHandling.Result<bool>>
	{
		public Guid Id { get; set; }

		public DeleteTourCommand(Guid id)
		{
			Id = id;
		}
	}
}

