using Core.feature.AntecedenteMedicos.Commands;
using Core.feature.AntecedenteMedicos.Queries;
using Core.feature.AtencionDiagnosticos.Commands;
using Core.feature.AtencionDiagnosticos.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtnDiagnosticosController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AtnDiagnosticosController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Archivos Clinicos PAGINADAS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<AtencionDiagnostico>>> Get(
            [FromQuery] GetAtnDiagnosticosQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Archivos Clinicos ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<AtencionDiagnostico>> GetById(int id)
        {
            var archivo = await _mediator.Send(
                new GetAtnDiagnosticosByIdQuery
                {
                    AtencionDiagnosticoId = id
                }
            );

            if (archivo == null)
            {
                return NotFound();
            }

            return Ok(archivo);
        }


        // =========================================
        // POST - CREAR Archivos Clinicos
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddAtnDiagnosticosCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Archivos Clinicos
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateAtnDiagnosticosCommand command)
        {
            command.AtencionDiagnosticoId = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Archivos Clinicos
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteAtnDiagnosticosCommand
                {
                    AtencionDiagnosticoId = id
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
