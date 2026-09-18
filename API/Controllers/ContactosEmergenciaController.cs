using Core.feature.ContactosEmergencia.Commands;
using Core.feature.ContactosEmergencia.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactosEmergenciaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactosEmergenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR CONTACTOS PAGINADOS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<ContactoEmergencia>>> Get(
            [FromQuery] GetContactosQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }
        // =========================================
        // GET - CONTACTO POR ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactoEmergencia>> GetById(
            int id)
        {
            var contacto = await _mediator.Send(
                new GetContactosByIdQuery
                {
                    IdContacto = id
                }
            );

            if (contacto == null)
            {
                return NotFound();
            }

            return Ok(contacto);
        }


        // =========================================
        // POST - CREAR CONTACTO
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddContactosCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }
    }
}