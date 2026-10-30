using Core.feature.ResultadoEstudios.Commands;
using Core.feature.ResultadoEstudios.Queries;
using Core.feature.SolicitudEstudios.Commands;
using Core.feature.SolicitudEstudios.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolicitudEstudioController : ControllerBase
    {

        private readonly IMediator _mediator;

        public SolicitudEstudioController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Solicitud Estudio
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<SolicitudEstudio>>> Get(
            [FromQuery] GetSolicitEstudQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Solicitud EstudioId
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultadoEstudio>> GetById(int id)
        {
            var solicitud = await _mediator.Send(
                new GetSolicitEstudByIdQuery
                {
                    IdSolicitudEstudio = id
                }
            );

            if (solicitud == null)
            {
                return NotFound();
            }

            return Ok(solicitud);
        }


        // =========================================
        // POST - CREAR Solicitud Estudio
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddSolicitEstudCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Solicitud Estudio
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateSolicEstudCommand command)
        {
            command.IdSolicitudEstudio = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Solicitud Estudio
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteSolicitEstudCommand
                {
                    IdSolicitudEstudio = id
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
