using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Domain.Enums;
using GetCategoriesByTypeQuery = OnlineTravel.Application.Features.Categories.GetCategoriesByType.GetCategoriesByTypeQuery;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/categories")]
public class CategoriesController : BaseApiController
{
    [HttpGet("by-type")]
    public async Task<ActionResult> GetByType([FromQuery] CategoryType type)
    {
        var result = await Mediator.Send(new GetCategoriesByTypeQuery(type));
        return HandleResult(result);
    }
}
