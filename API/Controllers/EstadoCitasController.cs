using Core.feature.EstadosCitas.Commands;
using Core.feature.EstadosCitas.Queires;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstadoCitasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EstadoCitasController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR ESTADOS PAGINADOS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<EstadoCitas>>> Get(
            [FromQuery] GetEstadosCitasQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - ESTADO POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoCitas>> GetById(int id)
        {
            var estado = await _mediator.Send(
                new GetEstadosCitasByIdQuery
                {
                    IdEstadoCitas = id
                }
            );

            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }


        // =========================================
        // POST - CREAR ESTADO
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddEstadosCitasCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR ESTADO
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateEstadoCitasCommand command)
        {
            command.IdEstadoCita = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR ESTADO
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteEstadoCitasCommand
                {
                    IdEstadoCita = id
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