using MediatR;
using OnlineTravel.Application.Common;

namespace OnlineTravel.Application.Features.Flights.Flights.Manage
{
	public record AddSeatCommand(Guid FlightId, string SeatLabel, string CabinClass, decimal ExtraCharge) : IRequest<Result<Guid>>;
	public record DeleteSeatCommand(Guid Id) : IRequest<Result<bool>>;
	public record AddFareCommand(Guid FlightId, string FareName, string Description, decimal Amount, string Currency, int SeatsAvailable) : IRequest<Result<Guid>>;
	public record DeleteFareCommand(Guid Id) : IRequest<Result<bool>>;
}
