using MediatR;
using OnlineTravel.Application.Features.Flights.CreateAirport;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;

namespace OnlineTravel.Application.Features.Flights.Airport.CreateAirport
{
	public class CreateAirportHandler : IRequestHandler<CreateAirportCommand, Result<CreateAirportResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateAirportHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CreateAirportResponse>> Handle(CreateAirportCommand request, CancellationToken cancellationToken)
		{
			//  Initialize Value Objects required for the Entity
			var iataCode = new IataCode(request.Code);
			var address = new Address(
				request.Street,
				request.City,
				request.State,
				request.Country,
				request.ZipCode);

			//  Map Command data to Airport Entity
			var airport = new OnlineTravel.Domain.Entities.Flights.Airport
			{
				Code = iataCode,
				Name = request.Name,
				Address = address,
				Facilities = request.Facilities ?? new List<string>(),
				IsActive = true
			};

			//  Add the entity to the database via the Unit of Work's Repository
			await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport>().AddAsync(airport);

			// Persist changes to the database (Commit)
			var result = await _unitOfWork.Complete();

			if (result <= 0)
			{
				return Result<CreateAirportResponse>.Failure(Error.InternalServer("Failed to create airport."));
			}

			// Map the created entity back to Response DTO
			var response = new CreateAirportResponse
			{
				Id = airport.Id,
				Name = airport.Name,
				Code = airport.Code.Value // Extracting the string value from the Value Object
			};

			return Result<CreateAirportResponse>.Success(response);
		}
	}
}
