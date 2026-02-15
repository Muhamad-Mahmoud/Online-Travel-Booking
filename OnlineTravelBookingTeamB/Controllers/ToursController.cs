using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Application.Features.Tours.GetAllTours.Queries;
using OnlineTravel.Application.Features.Tours.GetAllTours.DTOs;

namespace OnlineTravelBookingTeamB.Controllers;

public class ToursController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TourResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllToursQuery());
        return Ok(result);
    }

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
