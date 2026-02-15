using MediatR;

namespace OnlineTravel.Application.Features.Tours.DeleteTour.Commands
{
    public class DeleteTourCommand : IRequest
    {
        public Guid Id { get; set; }

        public DeleteTourCommand(Guid id)
        {
            Id = id;
        }
    }
}
