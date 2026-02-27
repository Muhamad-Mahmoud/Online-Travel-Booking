using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.Carrier.DeleteCarrier
{
    public class DeleteCarrierHandler : IRequestHandler<DeleteCarrierCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCarrierHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteCarrierCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>();
            var carrier = await repository.GetByIdAsync(request.Id);

            if (carrier == null)
            {
                return Result<bool>.Failure(Error.NotFound("Carrier not found"));
            }

            repository.Delete(carrier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
