using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

namespace OnlineTravelBookingTeamB.Controllers;

public class ToursController : BaseApiController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TourDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetTourByIdQuery(Id: id);
        var result = await Mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
