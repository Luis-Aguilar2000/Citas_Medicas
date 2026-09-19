using Core.Facturacion.Features.FormasPago.Commands;
using Core.Facturacion.Features.FormasPago.Queries;
using Domain.Facturacion.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormasPagoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FormasPagoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<FormaPago>>> Get(
            [FromQuery] GetFormasPagoQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormaPago>> GetById(int id)
        {
            var resultado = await _mediator.Send(
                new GetFormaPagoByIdQuery
                {
                    IdFormaPago = id
                });

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<FormaPago>> Post(
            AddFormaPagoCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            UpdateFormaPagoCommand command)
        {
            command.IdFormaPago = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteFormaPagoCommand
                {
                    IdFormaPago = id
                });

            if (!resultado)
                return NotFound();

            return NoContent();
        }
    }
}