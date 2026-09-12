using Core.feature.Consultorios.Commands;
using Core.feature.Consultorios.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultoriosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultoriosController(
            IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR CONSULTORIOS
        // =========================================

        [HttpGet]
        public async Task<List<Consultorio>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(
                new GetConsultorioQuery
                {
                    TotalRegistros = totalRegistros
                }
            );
        }


        // =========================================
        // GET - CONSULTORIO POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Consultorio>> GetById(
            int id)
        {
            var consultorio =
                await _mediator.Send(
                    new GetConsultorioByIdQuery
                    {
                        IdConsultorio = id
                    }
                );

            if (consultorio == null)
            {
                return NotFound();
            }

            return Ok(consultorio);
        }


        // =========================================
        // POST - CREAR CONSULTORIO
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddConsultorioCommand command)
        {
            var resultado =
                await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR CONSULTORIO
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateConsultorioCommand command)
        {
            command.IdConsultorio = id;

            var resultado =
                await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR CONSULTORIO
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(
            int id)
        {
            var resultado =
                await _mediator.Send(
                    new DeleteConsultorioCommand
                    {
                        IdConsultorio = id
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