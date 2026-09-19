using Core.Facturacion.Features.Servicios.Commands;
using Core.Facturacion.Features.Servicios.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiciosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Servicio>>> Get(
            [FromQuery] GetServiciosQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Servicio>> GetById(int id)
        {
            var resultado = await _mediator.Send(
                new GetServicioByIdQuery
                {
                    IdServicio = id
                });

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<Servicio>> Post(
            AddServicioCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            UpdateServicioCommand command)
        {
            command.IdServicio = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteServicioCommand
                {
                    IdServicio = id
                });

            if (!resultado)
                return NotFound();

            return NoContent();
        }
    }
}