using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using NetTopologySuite.Geometries;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateCoordinates;

public class UpdateTourCoordinatesCommandHandler : IRequestHandler<UpdateTourCoordinatesCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTourCoordinatesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTourCoordinatesCommand request, CancellationToken cancellationToken)
    {
        var tourRepository = _unitOfWork.Repository<Tour>();
        var tour = await tourRepository.GetByIdAsync(request.TourId, cancellationToken);

        if (tour == null)
        {
            return false;
        }

        // Create new address with updated coordinates, preserving existing address fields
        var existingAddress = tour.Address;
        Point? newCoordinates = null;
        
        if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            newCoordinates = new Point(request.Longitude.Value, request.Latitude.Value) { SRID = 4326 };
        }

        var newAddress = new Address(
            existingAddress?.Street,
            existingAddress?.City,
            existingAddress?.State,
            existingAddress?.Country,
            existingAddress?.PostalCode,
            newCoordinates
        );

        tour.UpdateAddress(newAddress);
        
        await _unitOfWork.Complete();
        return true;
    }
}
