using MediatR;
using OnlineTravel.Application.Features.Tours.DeleteTour.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.DeleteTour.Handlers
{
    public class DeleteTourCommandHandler : IRequestHandler<DeleteTourCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTourCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteTourCommand request, CancellationToken cancellationToken)
        {
            var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.Id);
            
            if (tour == null)
            {
                // Handle not found
                throw new KeyNotFoundException($"Tour with ID {request.Id} not found.");
            }

            _unitOfWork.Repository<Tour>().Delete(tour);
            await _unitOfWork.Complete();
        }
    }
}
