using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.Airport.DeleteAirport
{
    public class DeleteAirportHandler : IRequestHandler<DeleteAirportCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAirportHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteAirportCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport>();
            var airport = await repository.GetByIdAsync(request.Id);

            if (airport == null)
            {
                return Result<bool>.Failure(Error.NotFound("Airport not found"));
            }

            repository.Delete(airport);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
