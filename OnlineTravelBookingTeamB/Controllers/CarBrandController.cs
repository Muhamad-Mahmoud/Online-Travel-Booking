using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarBrands.Commands;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Application.Features.CarBrands.Queries;
using OnlineTravelBookingTeamB.Extensions;
using System.Net;
using static Microsoft.TeamFoundation.Framework.Common.AadSecurity;

namespace OnlineTravelBookingTeamB.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class CarBrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarBrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll_CarBrands")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCarBrandsQuery();
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("GetCarBrandsPaginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var query = new GetCarBrandsPaginatedQuery(pageIndex, pageSize, searchTerm);
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("Get_CarBrand_By/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCarBrandByIdQuery(id);
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("Add_CarBrand")]

        public async Task<IActionResult> Add(CreateCarBrandDto request)
        {
            var command = new CreateCarBrandCommand(request);
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById),
                    new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }
        
        [HttpPut("Update_CarBrand/{id:guid}")]

        public async Task<IActionResult> Update(Guid id, UpdateCarBrandDto request)
        {
            var command = new UpdateCarBrandCommand(id, request);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpDelete("Delete_CarBrand/{id:guid}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCarBrandCommand(id);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
