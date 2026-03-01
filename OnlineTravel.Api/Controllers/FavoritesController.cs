using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Favorites.AddFavorite;
using OnlineTravel.Application.Features.Favorites.DTOs;
using OnlineTravel.Application.Features.Favorites.GetUserFavorites;

namespace OnlineTravel.Api.Controllers;

[Authorize]
[Route("api/v1/favorites")]
public class FavoritesController : BaseApiController
{
	/// <summary>
	/// Add a destination or item to the user's favorites.
	/// </summary>
	[HttpPost]
	public async Task<ActionResult> Add([FromBody] AddFavoriteRequest request)
	{
		var command = new AddFavoriteCommand(UserId, request.ItemId);
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Get the list of items favorited by the authenticated user.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult> Get()
	{
		var query = new GetUserFavoritesQuery(UserId);
		var result = await Mediator.Send(query);
		return HandleResult(result);
	}
}

