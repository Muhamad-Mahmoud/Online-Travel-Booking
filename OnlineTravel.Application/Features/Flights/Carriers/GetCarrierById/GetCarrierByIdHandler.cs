using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flights.Carrier.GetCarrierById
{
	public class GetCarrierByIdHandler : IRequestHandler<GetCarrierByIdQuery, Result<GetCarrierByIdResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCarrierByIdHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<GetCarrierByIdResponse>> Handle(GetCarrierByIdQuery request, CancellationToken cancellationToken)
		{
			var carrier = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().GetByIdAsync(request.Id);
			if (carrier == null)
			{
				return Result<GetCarrierByIdResponse>.Failure(Error.NotFound($"Carrier with id '{request.Id}' was not found."));
			}

			return Result<GetCarrierByIdResponse>.Success(new GetCarrierByIdResponse
			{
				Id = carrier.Id,
				Name = carrier.Name,
				Code = carrier.Code.Value,
				Logo = carrier.Logo,
				IsActive = carrier.IsActive
			});
		}
	}
}
