using Core.Facturacion.Features.FacturaDetalles.Commands;
using Core.Facturacion.Features.FacturaDetalles.Queries;
using Core.Features.FacturaDetalles.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturaDetallesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacturaDetallesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<FacturaDetalle>>> Get(
            [FromQuery] GetFacturaDetallesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaDetalle>> GetById(int id)
        {
            var resultado = await _mediator.Send(
                new GetFacturaDetalleByIdQuery
                {
                    IdDetalle = id
                });

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<FacturaDetalle>> Post(
            AddFacturaDetalleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            UpdateFacturaDetalleCommand command)
        {
            command.IdDetalle = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteFacturaDetalleCommand
                {
                    IdDetalle = id
                });

            if (!resultado)
                return NotFound();

            return NoContent();
        }
    }
}