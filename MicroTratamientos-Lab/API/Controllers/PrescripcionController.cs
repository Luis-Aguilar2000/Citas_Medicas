using Core.feature.Prescripciones.Commands;
using Core.feature.Prescripciones.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        // GET - LISTAR PRESCRIPCIONES
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<Prescripcion>>> Get(
            [FromQuery] GetPrescripQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }

        // =========================================
        // GET - OBTENER PRESCRIPCIÓN POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Prescripcion>> GetById(int id)
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
        // POST - CREAR PRESCRIPCIÓN
        // =========================================

        [HttpPost]
        public async Task<ActionResult<int>> Post(
            [FromBody] AddPrescripCommand command)
        {
            var idPrescripcion = await _mediator.Send(command);

            return Ok(idPrescripcion);
        }

        // =========================================
        // PUT - ACTUALIZAR PRESCRIPCIÓN
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
        // DELETE - ELIMINAR PRESCRIPCIÓN
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