using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flights.Airport.GetAirportById
{
	public class GetAirportByIdHandler : IRequestHandler<GetAirportByIdQuery, Result<GetAirportByIdDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAirportByIdHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<GetAirportByIdDto>> Handle(GetAirportByIdQuery request, CancellationToken cancellationToken)
		{
			var airport = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport>().GetByIdAsync(request.Id);
			if (airport == null)
			{
				return Result<GetAirportByIdDto>.Failure(Error.NotFound($"Airport with id '{request.Id}' was not found."));
			}

			return Result<GetAirportByIdDto>.Success(new GetAirportByIdDto
			{
				Id = airport.Id,
				Name = airport.Name,
				Code = airport.Code.Value,
				FullAddress = $"{airport.Address.Street}, {airport.Address.City}, {airport.Address.Country}",
				Facilities = airport.Facilities
			});
		}
	}
}
