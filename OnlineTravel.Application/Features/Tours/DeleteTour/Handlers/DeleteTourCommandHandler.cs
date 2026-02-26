using MediatR;
using OnlineTravel.Application.Features.Tours.DeleteTour.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Tours.DeleteTour.Handlers
{
    public class DeleteTourCommandHandler : IRequestHandler<DeleteTourCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTourCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteTourCommand request, CancellationToken cancellationToken)
        {
            var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.Id);
            if (tour == null)
            {
                return Result<bool>.Failure(Error.NotFound($"Tour with id '{request.Id}' was not found."));
            }

            _unitOfWork.Repository<Tour>().Delete(tour);
            var affectedRows = await _unitOfWork.Complete();
            if (affectedRows <= 0)
            {
                return Result<bool>.Failure(Error.InternalServer("Failed to delete tour."));
            }

            return Result<bool>.Success(true);
        }
    }
}
