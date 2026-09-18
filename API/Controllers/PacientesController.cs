using Core.feature.Paciente.Commands;
using Core.feature.Paciente.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacientesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR PACIENTES PAGINADOS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<Pacientes>>> Get(
            [FromQuery] GetPacienteQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }

        // =========================================
        // GET - PACIENTE POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Pacientes>> GetById(
            int id)
        {
            var paciente = await _mediator.Send(
                new GetPacienteByIdQuery
                {
                    IdPaciente = id
                }
            );

            if (paciente == null)
            {
                return NotFound();
            }

            return Ok(paciente);
        }


        // =========================================
        // POST - CREAR PACIENTE
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddPacienteCommand command)
        {
            var resultado =
                await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR PACIENTE
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdatePacienteCommand command)
        {
            command.IdPaciente = id;

            var resultado =
                await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR PACIENTE
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(
            int id)
        {
            var resultado = await _mediator.Send(
                new DeletePacienteCommand
                {
                    IdPaciente = id
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