using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Entities.Favorites;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Favorites.Commands.AddFavorite;

public sealed class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddFavoriteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        CategoryType? foundType = null;

        // 1. Attempt to find the item in different repositories
        // Check Car
        if (await _unitOfWork.Repository<Car>().GetByIdAsync(request.ItemId, cancellationToken) != null)
        {
            foundType = CategoryType.Car;
        }
        // Check Tour
        else if (await _unitOfWork.Repository<Tour>().GetByIdAsync(request.ItemId, cancellationToken) != null)
        {
            foundType = CategoryType.Tour;
        }
        // Check Flight
        else if (await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>().GetByIdAsync(request.ItemId, cancellationToken) != null)
        {
            foundType = CategoryType.Flight;
        }
        // Check Room (Hotel)
        else if (await _unitOfWork.Repository<Room>().GetByIdAsync(request.ItemId, cancellationToken) != null)
        {
            foundType = CategoryType.Hotel;
        }

        if (foundType == null)
        {
            return Result.Failure<Guid>(new Error("ItemNotFound", $"Item with ID {request.ItemId} was not found.", 404));
        }

        var itemType = foundType.Value;

        // 2. Check if already favorited
        var existingFavorite = await _unitOfWork.Repository<Favorite>()
            .FindAsync(f => f.UserId == request.UserId && f.ItemId == request.ItemId && f.ItemType == itemType, cancellationToken);
        
        if (existingFavorite != null)
        {
            return Result.Failure<Guid>(new Error("DuplicateFavorite", "This item is already in your favorites.", 409));
        }

        // 3. Create and Save
        var favorite = new Favorite
        {
            UserId = request.UserId,
            ItemId = request.ItemId,
            ItemType = itemType,
            AddedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Favorite>().AddAsync(favorite, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(favorite.Id);
    }
}
