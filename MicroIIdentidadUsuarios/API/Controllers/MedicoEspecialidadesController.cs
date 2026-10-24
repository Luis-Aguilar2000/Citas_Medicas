using Core.feature.MedicoEspecialidad.Commands;
using Core.feature.MedicoEspecialidad.Queries;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MedicoEspecialidadesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicoEspecialidadesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] RequestParametersGets request)
        {
            var result = await _mediator.Send(
                new GetMedicoEspecialidadQuery
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    Filter = request.Filter
                });

            return Ok(result);
        }

        [HttpGet("{idMedico}/{idEspecialidad}")]
        public async Task<IActionResult> GetById(
            int idMedico,
            int idEspecialidad)
        {
            var result = await _mediator.Send(
                new GetMedicoEspecialidadByIdQuery
                {
                    IdMedico = idMedico,
                    IdEspecialidad = idEspecialidad
                });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] AddMedicoEspecialidadCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UpdateMedicoEspecialidadCommand command)
        {
            var result = await _mediator.Send(command);

            return result ? Ok(result) : NotFound();
        }

        [HttpDelete("{idMedico}/{idEspecialidad}")]
        public async Task<IActionResult> Delete(
            int idMedico,
            int idEspecialidad)
        {
            var result = await _mediator.Send(
                new DeleteMedicoEspecialidadCommand
                {
                    IdMedico = idMedico,
                    IdEspecialidad = idEspecialidad
                });

            return result ? Ok(result) : NotFound();
        }
    }
}