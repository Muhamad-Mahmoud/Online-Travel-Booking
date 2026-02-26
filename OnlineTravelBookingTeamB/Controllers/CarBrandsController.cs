using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarBrands.CreateCarBrand;
using OnlineTravel.Application.Features.CarBrands.DeleteCarBrand;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandById;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated;
using OnlineTravel.Application.Features.CarBrands.UpdateCarBrand;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/car-brands")]
[Authorize(Roles = "Admin")]
public class CarBrandsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var result = await Mediator.Send(new GetCarBrandsPaginatedQuery(pageIndex, pageSize, searchTerm));
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetCarBrandByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCarBrandRequest request)
    {
        var result = await Mediator.Send(new CreateCarBrandCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCarBrandRequest request)
    {
        request.Id = id;
        var result = await Mediator.Send(new UpdateCarBrandCommand(id, request));
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteCarBrandCommand(id));
        return HandleResult(result);
    }
}
