using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Carrier.CreateCarrier;
using OnlineTravel.Application.Features.Flight.Carrier.GetCarrierById;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/flights/carriers")]
public class CarriersController : BaseApiController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCarrierCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return HandleResult(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetCarrierByIdQuery(id));
        return HandleResult(result);
    }
}
