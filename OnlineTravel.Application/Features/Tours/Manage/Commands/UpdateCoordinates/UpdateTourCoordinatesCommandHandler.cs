using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateCoordinates;

public class UpdateTourCoordinatesCommandHandler : IRequestHandler<UpdateTourCoordinatesCommand, Result<bool>>
{
	private readonly IUnitOfWork _unitOfWork;

	public UpdateTourCoordinatesCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<bool>> Handle(UpdateTourCoordinatesCommand request, CancellationToken cancellationToken)
	{
		var tourRepository = _unitOfWork.Repository<Tour>();
		var tour = await tourRepository.GetByIdAsync(request.TourId, cancellationToken);
		if (tour == null)
		{
			return Result<bool>.Failure(Error.NotFound($"Tour with id '{request.TourId}' was not found."));
		}

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
			newCoordinates);

		tour.UpdateAddress(newAddress);
		var affectedRows = await _unitOfWork.Complete();
		if (affectedRows <= 0)
		{
			return Result<bool>.Failure(Error.InternalServer("Failed to update tour coordinates."));
		}

		return Result<bool>.Success(true);
	}
}
