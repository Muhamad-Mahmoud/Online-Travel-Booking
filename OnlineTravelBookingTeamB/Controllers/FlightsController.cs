using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineTravel.Application.Features.Flight.Flights.CreateFlight;
using OnlineTravel.Application.Features.Flight.Flights.SearchFlights.Queries;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/flights")]
public class FlightsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> Search([FromQuery] SearchFlightsQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateFlightCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return HandleResult(result);

        return Ok(new { id = result.Value });
    }
}
