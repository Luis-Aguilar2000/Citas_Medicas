using Core.feature.Turnos.Commands;
using Core.feature.Turnos.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TurnosController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR TURNOS PAGINADOS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<Turno>>> Get(
            [FromQuery] GetTurnosQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }

        // =========================================
        // GET - TURNO POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Turno>> GetById(
            int id)
        {
            var turno = await _mediator.Send(
                new GetTurnosByIdQuery
                {
                    IdTurno = id
                }
            );

            if (turno == null)
            {
                return NotFound();
            }

            return Ok(turno);
        }


        // =========================================
        // POST - CREAR TURNO
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddTurnosCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }
    }
}