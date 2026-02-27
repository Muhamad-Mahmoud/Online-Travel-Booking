using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Flights.Flights.Manage;

public class AddSeatCommand : IRequest<Result<Guid>>
{
    public Guid FlightId { get; set; }
    public string SeatLabel { get; set; } = string.Empty;
    public string CabinClass { get; set; } = string.Empty;
    public decimal ExtraCharge { get; set; }
}

public class DeleteSeatCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}

public class AddFareCommand : IRequest<Result<Guid>>
{
    public Guid FlightId { get; set; }
    public decimal Amount { get; set; }
    public int SeatsAvailable { get; set; }
}

public class DeleteFareCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
