using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Tours.Manage.AddPriceTier;

public class AddTourPriceTierCommandHandler : IRequestHandler<AddTourPriceTierCommand, Result<Guid>>
{
	private readonly IUnitOfWork _unitOfWork;

	public AddTourPriceTierCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(AddTourPriceTierCommand request, CancellationToken cancellationToken)
	{
		var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
		if (tour == null)
		{
			return Result<Guid>.Failure(Error.NotFound($"Tour with id '{request.TourId}' was not found."));
		}

		var priceTier = new TourPriceTier
		{
			TourId = request.TourId,
			Name = request.Name,
			Price = new Money(request.Amount, request.Currency),
			Description = request.Description,
			IsActive = true
		};

		await _unitOfWork.Repository<TourPriceTier>().AddAsync(priceTier, cancellationToken);
		var affectedRows = await _unitOfWork.SaveChangesAsync();
		if (affectedRows <= 0)
		{
			return Result<Guid>.Failure(Error.InternalServer("Failed to add tour price tier."));
		}

		return Result<Guid>.Success(priceTier.Id);
	}
}
