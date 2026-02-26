using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.Airport.UpdateAirport
{
    public class UpdateAirportHandler : IRequestHandler<UpdateAirportCommand, Result<UpdateAirportResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAirportHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateAirportResponse>> Handle(UpdateAirportCommand request, CancellationToken cancellationToken)
        {
            var airport = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport>().GetByIdAsync(request.Id);
            if (airport == null)
            {
                return Result<UpdateAirportResponse>.Failure(Error.NotFound($"Airport with id '{request.Id}' was not found."));
            }

            airport.Name = request.Name;
            airport.Code = new IataCode(request.Code);
            airport.Address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            airport.Facilities = request.Facilities;

            _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport>().Update(airport);
            var affectedRows = await _unitOfWork.Complete();
            if (affectedRows <= 0)
            {
                return Result<UpdateAirportResponse>.Failure(Error.InternalServer("Failed to update airport."));
            }

            return Result<UpdateAirportResponse>.Success(new UpdateAirportResponse
            {
                Id = airport.Id,
                Name = airport.Name,
                IsSuccess = true
            });
        }
    }
}
