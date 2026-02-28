using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Features.Flights.Carriers.DeleteCarrier
{
	public class DeleteCarrierHandler : IRequestHandler<DeleteCarrierCommand, OnlineTravel.Application.Common.Result<bool>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteCarrierHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<OnlineTravel.Application.Common.Result<bool>> Handle(DeleteCarrierCommand request, CancellationToken cancellationToken)
		{
			var carrier = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().GetByIdAsync(request.Id);
			if (carrier == null) return OnlineTravel.Application.Common.Result<bool>.Failure("Carrier not found");

			_unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().Delete(carrier);
			await _unitOfWork.Complete();
			return OnlineTravel.Application.Common.Result<bool>.Success(true);
		}
	}
}

