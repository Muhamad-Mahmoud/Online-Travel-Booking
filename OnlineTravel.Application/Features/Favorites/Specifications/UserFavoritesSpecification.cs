using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Favorites;

namespace OnlineTravel.Application.Features.Favorites.Specifications;

/// <summary>
/// Specification to retrieve all favorites belonging to a specific user.
/// Pushes the WHERE clause to the database, avoiding a full-table scan.
/// </summary>
public sealed class UserFavoritesSpecification : BaseSpecification<Favorite>
{
    public UserFavoritesSpecification(Guid userId)
        : base(f => f.UserId == userId)
    {
        AddOrderByDesc(f => f.AddedAt);
    }
}
