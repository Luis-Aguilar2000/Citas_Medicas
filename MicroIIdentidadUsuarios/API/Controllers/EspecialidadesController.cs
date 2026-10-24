using Core.feature.Especialidad.Commands;
using Core.feature.Especialidad.Queries;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EspecialidadesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] RequestParametersGets request)
        {
            var result = await _mediator.Send(new GetEspecialidadQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Filter = request.Filter
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetEspecialidadByIdQuery
            {
                IdEspecialidad = id
            });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] AddEspecialidadCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UpdateEspecialidadCommand command)
        {
            var result = await _mediator.Send(command);

            return result ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEspecialidadCommand
            {
                IdEspecialidad = id
            });

            return result ? Ok(result) : NotFound();
        }
    }
}