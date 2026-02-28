using MediatR;

namespace OnlineTravel.Application.Features.Flights.Carrier.CreateCarrier
{
	public class CreateCarrierCommand : IRequest<OnlineTravel.Domain.ErrorHandling.Result<Guid>>
	{
		public string Name { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty; // IataCode
		public string? Logo { get; set; }

		// Contact Info details (assuming it's a Value Object or simple properties)
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;

	}
}

