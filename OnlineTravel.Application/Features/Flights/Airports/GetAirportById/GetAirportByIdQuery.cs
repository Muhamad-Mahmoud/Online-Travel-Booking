using MediatR;

namespace OnlineTravel.Application.Features.Flights.Airport.GetAirportById
{
	public class GetAirportByIdQuery : IRequest<OnlineTravel.Domain.ErrorHandling.Result<GetAirportByIdDto>>
	{
		public Guid Id { get; set; }

		public GetAirportByIdQuery(Guid id)
		{
			Id = id;
		}
	}
}

