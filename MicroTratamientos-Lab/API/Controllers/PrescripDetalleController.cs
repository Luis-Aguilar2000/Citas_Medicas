using Core.feature.Prescripciones.Commands;
using Core.feature.Prescripciones.Queries;
using Core.feature.PrescripcionesDetalles.Commands;
using Core.feature.PrescripcionesDetalles.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

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
        // GET - LISTAR PrescripciónDetalle
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<PrescripcionDetalle>>> Get(
            [FromQuery] GetPrescripDetalleQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET -  PrescripciónDetalleId
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> GetById(int id)
        {
            var prescripcion = await _mediator.Send(
                new GetPrescripDetalleByIdQuery
                {
                    IdPrescripcionDetalle = id
                }
            );

            if (prescripcion == null)
            {
                return NotFound();
            }

            return Ok(prescripcion);
        }


        // =========================================
        // POST - CREAR PrescripciónDetalle
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddPrescripCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR PrescripciónDetalle
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
        // DELETE - ELIMINAR PrescripciónDetalle
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
