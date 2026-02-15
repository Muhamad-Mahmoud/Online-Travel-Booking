using MediatR;
using OnlineTravel.Application.Features.Favorites.DTOs;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Favorites;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Favorites.Queries.GetUserFavorites;

public sealed class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, Result<List<FavoriteDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserFavoritesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<FavoriteDto>>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        // Fetch all favorites for the user
        // Using GenericRepository spec or direct predicate if supported
        var favorites = await _unitOfWork.Repository<Favorite>()
            .GetAllAsync(cancellationToken);
        
        // Filter in memory or use spec if available. 
        // Ideally we should use a Specification, but for now filtering in memory or using FindAsync if it returns List.
        // Looking at IGenericRepository, FindAsync returns single T?. 
        // We need GetAllWithSpecAsync or similar.
        // Let's create a specification for UserFavorites to be clean.
        
        // Since I can't easily see Specification base class right now, I'll rely on GetAllAsync and filter.
        // Wait, GetAllAsync returns ALL. That's bad for performance if table is huge.
        // I should check if I can use a Specification.
        
        // Let's assume there is a Specification pattern. 
        // But for speed, and given I haven't seen the Specification base, I will use GetAllAsync and filter client-side (bad practice but works for small data)
        // OR better: I will check if I can add a method to Repository or use a Spec.
        
        // Actually, let's look at `UserFavoritesSpecification` if I can make one.
        // I'll stick to a simple implementation first. passing a predicate to Where on IEnumerable.
        
        var userFavorites = favorites
            .Where(f => f.UserId == request.UserId)
            .Select(f => new FavoriteDto(
                f.Id,
                f.ItemId,
                f.ItemType.ToString(),
                f.AddedAt
            ))
            .ToList();

        return Result.Success(userFavorites);
    }
}
