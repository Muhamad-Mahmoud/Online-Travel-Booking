using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Tours.Manage.AddActivity;

public class AddTourActivityCommandHandler : IRequestHandler<AddTourActivityCommand, Result<Guid>>
{
	private readonly IUnitOfWork _unitOfWork;

	public AddTourActivityCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(AddTourActivityCommand request, CancellationToken cancellationToken)
	{
		var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
		if (tour == null)
		{
			return Result<Guid>.Failure(Error.NotFound($"Tour with id '{request.TourId}' was not found."));
		}

		var activity = new TourActivity
		{
			TourId = request.TourId,
			Title = request.Title,
			Description = request.Description,
			Image = new ImageUrl(request.ImageUrl)
		};

		await _unitOfWork.Repository<TourActivity>().AddAsync(activity, cancellationToken);
		var affectedRows = await _unitOfWork.SaveChangesAsync();
		if (affectedRows <= 0)
		{
			return Result<Guid>.Failure(Error.InternalServer("Failed to add tour activity."));
		}

		return Result<Guid>.Success(activity.Id);
	}
}
