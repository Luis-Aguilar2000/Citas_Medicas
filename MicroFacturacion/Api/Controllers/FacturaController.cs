using Core.Facturacion.Features.Facturas.Commands;
using Core.Facturacion.Features.Facturas.Queries;
using Domain.Facturacion.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacturasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Factura>>> Get(
            [FromQuery] GetFacturasQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetById(int id)
        {
            var resultado = await _mediator.Send(
                new GetFacturaByIdQuery
                {
                    IdFactura = id
                });

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<Factura>> Post(
            AddFacturaCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            UpdateFacturaCommand command)
        {
            command.IdFactura = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteFacturaCommand
                {
                    IdFactura = id
                });

            if (!resultado)
                return NotFound();

            return NoContent();
        }
    }
}