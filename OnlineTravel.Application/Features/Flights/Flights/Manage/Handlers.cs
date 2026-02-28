using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Flights.Flights.Manage;

public class ManageFlightHandlers(IUnitOfWork unitOfWork) : 
    IRequestHandler<AddSeatCommand, OnlineTravel.Application.Common.Result<Guid>>,
    IRequestHandler<DeleteSeatCommand, OnlineTravel.Application.Common.Result<bool>>,
    IRequestHandler<AddFareCommand, OnlineTravel.Application.Common.Result<Guid>>,
    IRequestHandler<DeleteFareCommand, OnlineTravel.Application.Common.Result<bool>>
{


    public async Task<OnlineTravel.Application.Common.Result<Guid>> Handle(AddSeatCommand request, CancellationToken cancellationToken)

    {
        var flight = await unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>()
            .Query()
            .FirstOrDefaultAsync(f => f.Id == request.FlightId, cancellationToken);

        if (flight == null) return OnlineTravel.Application.Common.Result<Guid>.Failure("Flight not found.");


        if (!Enum.TryParse<CabinClass>(request.CabinClass, out var cabinClass))
            return OnlineTravel.Application.Common.Result<Guid>.Failure("Invalid cabin class.");


        var seat = new FlightSeat
        {
            FlightId = request.FlightId,
            SeatLabel = request.SeatLabel,
            CabinClass = cabinClass,
            ExtraCharge = request.ExtraCharge,
            IsAvailable = true
        };

        await unitOfWork.Repository<FlightSeat>().AddAsync(seat);
        await unitOfWork.Complete();

        return OnlineTravel.Application.Common.Result<Guid>.Success(seat.Id);
    }


    public async Task<OnlineTravel.Application.Common.Result<bool>> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)

    {
        var seat = await unitOfWork.Repository<FlightSeat>().GetByIdAsync(request.Id);
        if (seat == null) return OnlineTravel.Application.Common.Result<bool>.Failure("Seat not found.");

        unitOfWork.Repository<FlightSeat>().Delete(seat);
        var affected = await unitOfWork.Complete();
        return OnlineTravel.Application.Common.Result<bool>.Success(affected > 0);
    }


    public async Task<OnlineTravel.Application.Common.Result<Guid>> Handle(AddFareCommand request, CancellationToken cancellationToken)

    {
        var flight = await unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>()
            .Query()
            .FirstOrDefaultAsync(f => f.Id == request.FlightId, cancellationToken);

        if (flight == null) return OnlineTravel.Application.Common.Result<Guid>.Failure("Flight not found.");


        var fare = new FlightFare
        {
            FlightId = request.FlightId,
            BasePrice = new OnlineTravel.Domain.Entities._Shared.ValueObjects.Money(request.Amount),
            SeatsAvailable = request.SeatsAvailable
        };

        await unitOfWork.Repository<FlightFare>().AddAsync(fare);
        await unitOfWork.Complete();

        return OnlineTravel.Application.Common.Result<Guid>.Success(fare.Id);
    }


    public async Task<OnlineTravel.Application.Common.Result<bool>> Handle(DeleteFareCommand request, CancellationToken cancellationToken)

    {
        var fare = await unitOfWork.Repository<FlightFare>().GetByIdAsync(request.Id);
        if (fare == null) return OnlineTravel.Application.Common.Result<bool>.Failure("Fare not found.");

        unitOfWork.Repository<FlightFare>().Delete(fare);
        var affected = await unitOfWork.Complete();
        return OnlineTravel.Application.Common.Result<bool>.Success(affected > 0);
    }

}
