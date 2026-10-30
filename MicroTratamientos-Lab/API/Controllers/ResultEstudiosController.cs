using Core.feature.Prescripciones.Commands;
using Core.feature.PrescripcionesDetalles.Commands;
using Core.feature.PrescripcionesDetalles.Queries;
using Core.feature.ResultadoEstudios.Commands;
using Core.feature.ResultadoEstudios.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResultEstudiosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResultEstudiosController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Resultado Estudio
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<ResultadoEstudio>>> Get(
            [FromQuery] GetResultEstudiosQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Resultado EstudioId
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultadoEstudio>> GetById(int id)
        {
            var estudio = await _mediator.Send(
                new GetResultEstudiosByIdQuery
                {
                    IdResultadoEstudio = id
                }
            );

            if (estudio == null)
            {
                return NotFound();
            }

            return Ok(estudio);
        }


        // =========================================
        // POST - CREAR Resultado Estudio
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddResultEstudioCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Resultado Estudio
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateResultEstudioCommand command)
        {
            command.IdResultadoEstudio = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Resultado Estudio
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteResultEstudioCommand
                {
                    IdResultadoEstudio = id
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
