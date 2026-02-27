using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateTour;

public class UpdateTourCommandHandler : IRequestHandler<UpdateTourCommand, Result<bool>>
{
	private readonly IUnitOfWork _unitOfWork;

	public UpdateTourCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<bool>> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
	{
		var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
		if (tour == null)
		{
			return Result<bool>.Failure(Error.NotFound($"Tour with id '{request.TourId}' was not found."));
		}

		tour.Title = request.Title;
		tour.Description = request.Description;
		tour.CategoryId = request.CategoryId;
		tour.DurationDays = request.DurationDays;
		tour.DurationNights = request.DurationNights;
		tour.Recommended = request.Recommended;
		tour.BestTimeToVisit = request.BestTimeToVisit;

		tour.UpdateAddress(new Address(
			request.Street, request.City, request.State, request.Country, request.PostalCode,
			tour.Address?.Coordinates));

		if (!string.IsNullOrEmpty(request.MainImageUrl))
		{
			tour.MainImage = new ImageUrl(request.MainImageUrl);
		}

		_unitOfWork.Repository<Tour>().Update(tour);
		var affectedRows = await _unitOfWork.Complete();
		if (affectedRows <= 0)
		{
			return Result<bool>.Failure(Error.InternalServer("Failed to update tour."));
		}

		return Result<bool>.Success(true);
	}
}
