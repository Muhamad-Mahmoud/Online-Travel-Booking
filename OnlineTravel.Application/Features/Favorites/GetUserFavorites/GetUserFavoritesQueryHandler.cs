using MediatR;
using OnlineTravel.Application.Features.Favorites.DTOs;
using OnlineTravel.Application.Features.Favorites.Specifications;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Favorites;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Favorites.GetUserFavorites;

public sealed class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, Result<List<FavoriteDto>>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetUserFavoritesQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<List<FavoriteDto>>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
	{
		// Use a specification so the WHERE UserId = @id filter runs on the database,
		// not in C# memory after loading the entire Favorites table.
		var spec = new UserFavoritesSpecification(request.UserId);
		var favorites = await _unitOfWork.Repository<Favorite>()
			.GetAllWithSpecAsync(spec, cancellationToken);

		var dtos = favorites
			.Select(f => new FavoriteDto(f.Id, f.ItemId, f.ItemType.ToString(), f.AddedAt))
			.ToList();

		return Result.Success(dtos);
	}
}
