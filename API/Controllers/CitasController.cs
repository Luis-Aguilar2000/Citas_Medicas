using Core.feature.Citas.Commands;
using Core.feature.Citas.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitasController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR CITAS PAGINADAS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<Cita>>> Get(
            [FromQuery] GetCitasQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - CITA POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetById(int id)
        {
            var cita = await _mediator.Send(
                new GetCitasByIdQuery
                {
                    IdCitas = id
                }
            );

            if (cita == null)
            {
                return NotFound();
            }

            return Ok(cita);
        }


        // =========================================
        // POST - CREAR CITA
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddCitasCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR CITA
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateCitasCommand command)
        {
            command.IdCita = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR CITA
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteCitasCommand
                {
                    IdCita = id
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