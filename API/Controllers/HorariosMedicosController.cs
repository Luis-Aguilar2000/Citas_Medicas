using Core.feature.HorarioMedico.Commands;
using Core.feature.HorarioMedico.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HorariosMedicosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HorariosMedicosController(
            IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR HORARIOS PAGINADOS
        // =========================================
        [HttpGet]
        public async Task<ActionResult<PagedResult<HorariosMedico>>> Get(
            [FromQuery] GetHorarioMedicoQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - HORARIO POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<HorariosMedico>> GetById(
            int id)
        {
            var horario = await _mediator.Send(
                new GetHorarioMedicoByIdQuery
                {
                    IdHorario = id
                }
            );

            if (horario == null)
            {
                return NotFound();
            }

            return Ok(horario);
        }


        // =========================================
        // POST - CREAR HORARIO
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddHorarioMedicoCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR HORARIO
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateHorarioMedicoCommand command)
        {
            command.IdHorario = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR HORARIO
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(
            int id)
        {
            var resultado = await _mediator.Send(
                new DeleteHorarioMedicoCommand
                {
                    IdHorario = id
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