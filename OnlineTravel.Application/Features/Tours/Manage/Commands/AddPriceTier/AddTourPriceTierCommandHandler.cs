using MediatR;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier;

public class AddTourPriceTierCommandHandler : IRequestHandler<AddTourPriceTierCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddTourPriceTierCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddTourPriceTierCommand request, CancellationToken cancellationToken)
    {
        var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
        if (tour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
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
        await _unitOfWork.Complete();

        return priceTier.Id;
    }
}
