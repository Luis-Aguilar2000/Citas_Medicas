using Core.Facturacion.Exceptions;
using Core.Facturacion.Features.Facturas.Commands;
using Core.Facturacion.Features.Facturas.Queries;
using Domain.Models;
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
            [FromBody] AddFacturaCommand command)
        {
            try
            {
                var factura = await _mediator.Send(command);

                return Ok(factura);
            }
            catch (DomainException ex)
                when (ex.Error == Errores.PACIENTE_NO_EXISTE)
            {
                return BadRequest(new
                {
                    codigo = (int)ex.Error,
                    mensaje = ex.Message
                });
            }
            catch (DomainException ex)
                when (ex.Error == Errores.PACIENTES_NO_DISPONIBLE)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new
                    {
                        codigo = (int)ex.Error,
                        mensaje = ex.Message
                    });
            }
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