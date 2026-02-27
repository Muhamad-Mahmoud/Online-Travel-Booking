using MediatR;

namespace OnlineTravel.Application.Features.Flights.Carrier.GetCarrierById
{
	public class GetCarrierByIdQuery : IRequest<OnlineTravel.Domain.ErrorHandling.Result<GetCarrierByIdDto>>
	{
		public Guid Id { get; set; }
		public GetCarrierByIdQuery(Guid id) => Id = id;
	}
}

