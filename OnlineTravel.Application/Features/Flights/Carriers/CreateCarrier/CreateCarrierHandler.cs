using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flights.Carrier.CreateCarrier
{
	public class CreateCarrierHandler : IRequestHandler<CreateCarrierCommand, Result<Guid>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateCarrierHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<Guid>> Handle(CreateCarrierCommand request, CancellationToken cancellationToken)
		{
			// 1. Map to Entity using the full path to avoid conflicts
			var carrier = new OnlineTravel.Domain.Entities.Flights.Carrier
			{
				Name = request.Name,
				Code = new IataCode(request.Code),
				Logo = request.Logo,
				IsActive = true
				// Note: Add ContactInfo mapping here based on your Value Object structure
			};

			// 2. Add and Save
			await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().AddAsync(carrier);
			var affectedRows = await _unitOfWork.SaveChangesAsync();
			if (affectedRows <= 0)
			{
				return Result<Guid>.Failure(Error.InternalServer("Failed to create carrier."));
			}

			return Result<Guid>.Success(carrier.Id);
		}
	}
}
