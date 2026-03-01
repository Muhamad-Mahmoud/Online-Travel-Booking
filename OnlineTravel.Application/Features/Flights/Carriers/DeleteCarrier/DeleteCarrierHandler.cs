using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Features.Flights.Carriers.DeleteCarrier
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
			var carrier = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().GetByIdAsync(request.Id);
			if (carrier == null) return Result<bool>.Failure("Carrier not found");

			_unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().Delete(carrier);
			await _unitOfWork.SaveChangesAsync();
			return Result<bool>.Success(true);
		}
	}
}

