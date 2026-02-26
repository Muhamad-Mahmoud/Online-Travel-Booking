using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineTravel.Application.Features.Cars.CreateCar;
using OnlineTravel.Application.Features.Cars.DeleteCar;
using OnlineTravel.Application.Features.Cars.GetAllCars;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;
using OnlineTravel.Application.Features.Cars.UpdateCar;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/cars")]
public class CarsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] GetAllCarsQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpGet("summary")]
    public async Task<ActionResult> GetAllSummary([FromQuery] GetAllCarsSummaryQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult> GetByIdWithDetails(Guid id)
    {
        var query = new GetCarDetailsByIdQuery(id);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetCarByIdQuery(id));
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCarRequest request)
    {
        var result = await Mediator.Send(new CreateCarCommand(request));
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCarRequest request)
    {
        request.Id = id;
        var result = await Mediator.Send(new UpdateCarCommand(request));
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteCarCommand(id));
        return HandleResult(result);
    }
}
