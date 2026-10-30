using Core.feature.Medicamentos.Commands;
using Core.feature.Medicamentos.Queries;
using Core.feature.Prescripciones.Commands;
using Core.feature.Prescripciones.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrescripcionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public PrescripcionController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Prescripción
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<Prescripcion>>> Get(
            [FromQuery] GetPrescripQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Catalogo PrescripciónId
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> GetById(int id)
        {
            var prescripcion = await _mediator.Send(
                new GetPrescripByIdQuery
                {
                    IdPrescripcion = id
                }
            );

            if (prescripcion == null)
            {
                return NotFound();
            }

            return Ok(prescripcion);
        }


        // =========================================
        // POST - CREAR Prescripción
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddPrescripCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Prescripción
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdatePrescripCommand command)
        {
            command.IdPrescripcion = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Prescripción
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeletePrescripCommand
                {
                    IdPrescripcion = id
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
