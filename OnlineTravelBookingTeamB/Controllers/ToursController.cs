using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineTravel.Application.Features.Tours.CreateTour.Commands;
using OnlineTravel.Application.Features.Tours.DeleteTour.Commands;
using OnlineTravel.Application.Features.Tours.GetAllTours.Queries;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddActivity;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier;
using OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateCoordinates;
using OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateTour;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/tours")]
public class ToursController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] GetAllToursQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetTourByIdQuery(id));
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTourCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return HandleResult(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateTourCommand command)
    {
        command.TourId = id;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteTourCommand(id));
        if (!result.IsSuccess)
            return HandleResult(result);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/activities")]
    public async Task<ActionResult> AddActivity(Guid id, [FromBody] AddTourActivityCommand command)
    {
        command.TourId = id;
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return HandleResult(result);

        return CreatedAtAction(nameof(GetById), new { id }, new { id = result.Value });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/images")]
    public async Task<ActionResult> AddImage(Guid id, [FromBody] AddTourImageCommand command)
    {
        command.TourId = id;
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return HandleResult(result);

        return CreatedAtAction(nameof(GetById), new { id }, new { id = result.Value });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/price-tiers")]
    public async Task<ActionResult> AddPriceTier(Guid id, [FromBody] AddTourPriceTierCommand command)
    {
        command.TourId = id;
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return HandleResult(result);

        return CreatedAtAction(nameof(GetById), new { id }, new { id = result.Value });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/coordinates")]
    public async Task<ActionResult> UpdateCoordinates(Guid id, [FromBody] UpdateTourCoordinatesCommand command)
    {
        command.TourId = id;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
