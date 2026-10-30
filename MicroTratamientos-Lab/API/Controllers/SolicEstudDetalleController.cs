using Core.feature.ResultadoEstudios.Commands;
using Core.feature.ResultadoEstudios.Queries;
using Core.feature.SolicitudEstudios.Commands;
using Core.feature.SolicitudEstudios.Queries;
using Core.feature.SolicitusEstudiosDetallles.Commands;
using Core.feature.SolicitusEstudiosDetallles.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolicEstudDetalleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolicEstudDetalleController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Solicitud Estudio Detalle
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<SolicitudEstudioDetalle>>> Get(
            [FromQuery] GetSolicEstudDetallQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Solicitud EstudioDetalleId
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultadoEstudio>> GetById(int id)
        {
            var solicitudDetalle = await _mediator.Send(
                new GetSolicEstudDetallByIdQuery
                {
                    IdSolicitudEstudioDetalle = id
                }
            );

            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            return Ok(solicitudDetalle);
        }


        // =========================================
        // POST - CREAR Solicitud Estudio Detalle
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddSolictEstudDetallCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Solicitud Estudio Detalle
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateSolicEstudDetallCommand command)
        {
            command.IdSolicitudEstudioDetalle = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Solicitud Estudio Detalle
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteSolicEstudDetallCommand
                {
                    IdSolicitudEstudioDetalle = id
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
