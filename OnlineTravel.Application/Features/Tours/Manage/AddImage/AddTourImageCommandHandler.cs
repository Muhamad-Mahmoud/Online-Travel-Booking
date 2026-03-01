using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Tours.Manage.AddImage;

public class AddTourImageCommandHandler : IRequestHandler<AddTourImageCommand, Result<Guid>>
{
	private readonly IUnitOfWork _unitOfWork;

	public AddTourImageCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(AddTourImageCommand request, CancellationToken cancellationToken)
	{
		var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
		if (tour == null)
		{
			return Result<Guid>.Failure(Error.NotFound($"Tour with id '{request.TourId}' was not found."));
		}

		var image = new TourImage
		{
			TourId = request.TourId,
			Url = request.Url,
			AltText = request.AltText
		};

		await _unitOfWork.Repository<TourImage>().AddAsync(image, cancellationToken);
		var affectedRows = await _unitOfWork.SaveChangesAsync();
		if (affectedRows <= 0)
		{
			return Result<Guid>.Failure(Error.InternalServer("Failed to add tour image."));
		}

		return Result<Guid>.Success(image.Id);
	}
}
