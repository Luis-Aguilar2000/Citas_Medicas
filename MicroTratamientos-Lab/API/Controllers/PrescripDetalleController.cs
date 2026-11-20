using Core.feature.PrescripcionesDetalles.Commands;
using Core.feature.PrescripcionesDetalles.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrescripDetalleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescripDetalleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // =========================================
        // GET - LISTAR DETALLES DE PRESCRIPCIÓN
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<PrescripcionDetalle>>> Get(
            [FromQuery] GetPrescripDetalleQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }

        // =========================================
        // GET - OBTENER DETALLE POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescripcionDetalle>> GetById(int id)
        {
            var prescripcionDetalle = await _mediator.Send(
                new GetPrescripDetalleByIdQuery
                {
                    IdPrescripcionDetalle = id
                }
            );

            if (prescripcionDetalle == null)
            {
                return NotFound();
            }

            return Ok(prescripcionDetalle);
        }

        // =========================================
        // POST - CREAR DETALLE DE PRESCRIPCIÓN
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddPrescripDetalleCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }

        // =========================================
        // PUT - ACTUALIZAR DETALLE DE PRESCRIPCIÓN
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdatePrescripDetalleCommand command)
        {
            command.IdPrescripcionDetalle = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }

        // =========================================
        // DELETE - ELIMINAR DETALLE DE PRESCRIPCIÓN
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeletePrescripDetalleCommand
                {
                    IdPrescripcionDetalle = id
                }
            );

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }
    }
}