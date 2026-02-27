using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Flight.Flights.Manage;

public class ManageFlightHandlers : 
    IRequestHandler<AddSeatCommand, Result<Guid>>,
    IRequestHandler<DeleteSeatCommand, Result<bool>>,
    IRequestHandler<AddFareCommand, Result<Guid>>,
    IRequestHandler<DeleteFareCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ManageFlightHandlers(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddSeatCommand request, CancellationToken cancellationToken)
    {
        var flight = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>()
            .Query()
            .FirstOrDefaultAsync(f => f.Id == request.FlightId, cancellationToken);

        if (flight == null) return Result<Guid>.Failure(Error.NotFound("Flight not found."));

        if (!Enum.TryParse<CabinClass>(request.CabinClass, out var cabinClass))
            return Result<Guid>.Failure(Error.Validation("Invalid cabin class."));

        var seat = new FlightSeat
        {
            FlightId = request.FlightId,
            SeatLabel = request.SeatLabel,
            CabinClass = cabinClass,
            ExtraCharge = request.ExtraCharge,
            IsAvailable = true
        };

        await _unitOfWork.Repository<FlightSeat>().AddAsync(seat);
        await _unitOfWork.Complete();

        return Result<Guid>.Success(seat.Id);
    }

    public async Task<Result<bool>> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)
    {
        var seat = await _unitOfWork.Repository<FlightSeat>().GetByIdAsync(request.Id);
        if (seat == null) return Result<bool>.Failure(Error.NotFound("Seat not found."));

        _unitOfWork.Repository<FlightSeat>().Delete(seat);
        var affected = await _unitOfWork.Complete();
        return Result<bool>.Success(affected > 0);
    }

    public async Task<Result<Guid>> Handle(AddFareCommand request, CancellationToken cancellationToken)
    {
        var flight = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>()
            .Query()
            .FirstOrDefaultAsync(f => f.Id == request.FlightId, cancellationToken);

        if (flight == null) return Result<Guid>.Failure(Error.NotFound("Flight not found."));

        var fare = new FlightFare
        {
            FlightId = request.FlightId,
            BasePrice = new OnlineTravel.Domain.Entities._Shared.ValueObjects.Money(request.Amount),
            SeatsAvailable = request.SeatsAvailable
        };

        await _unitOfWork.Repository<FlightFare>().AddAsync(fare);
        await _unitOfWork.Complete();

        return Result<Guid>.Success(fare.Id);
    }

    public async Task<Result<bool>> Handle(DeleteFareCommand request, CancellationToken cancellationToken)
    {
        var fare = await _unitOfWork.Repository<FlightFare>().GetByIdAsync(request.Id);
        if (fare == null) return Result<bool>.Failure(Error.NotFound("Fare not found."));

        _unitOfWork.Repository<FlightFare>().Delete(fare);
        var affected = await _unitOfWork.Complete();
        return Result<bool>.Success(affected > 0);
    }
}
