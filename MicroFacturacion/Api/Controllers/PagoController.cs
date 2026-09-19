using Core.Facturacion.Features.Pagos.Commands;
using Core.Facturacion.Features.Pagos.Queries;
using Domain.Facturacion.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PagosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Pago>>> Get(
            [FromQuery] GetPagosQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetById(int id)
        {
            var resultado = await _mediator.Send(
                new GetPagoByIdQuery
                {
                    IdPago = id
                });

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<Pago>> Post(
            AddPagoCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            UpdatePagoCommand command)
        {
            command.IdPago = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeletePagoCommand
                {
                    IdPago = id
                });

            if (!resultado)
                return NotFound();

            return NoContent();
        }
    }
}