using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Favorites.Commands.AddFavorite;
using OnlineTravel.Application.Features.Favorites.DTOs;
using OnlineTravel.Application.Features.Favorites.Queries.GetUserFavorites;
using OnlineTravelBookingTeamB.Extensions;

namespace OnlineTravelBookingTeamB.Controllers;

[Authorize]
public class FavoritesController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> Add([FromBody] AddFavoriteRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var command = new AddFavoriteCommand(userId, request.ItemId);
        
        var result = await Mediator.Send(command);

        return result.ToResponse(201);
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetUserFavoritesQuery(userId);
        var result = await Mediator.Send(query);

        return result.ToResponse();
    }
}
