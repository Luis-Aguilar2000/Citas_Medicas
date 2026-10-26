using Core.feature.Usuario.Commands;
using Core.feature.Usuario.Queries;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] RequestParametersGets request)
        {
            var result = await _mediator.Send(new GetUsuarioQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Filter = request.Filter
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetUsuarioByIdQuery
                {
                    IdUsuario = id
                }
            );

            return result == null
                ? NotFound()
                : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] AddUsuarioCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UpdateUsuarioCommand command)
        {
            var result = await _mediator.Send(command);

            return result
                ? Ok(result)
                : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(
                new DeleteUsuarioCommand
                {
                    IdUsuario = id
                }
            );

            return result
                ? Ok(result)
                : NotFound();
        }
    }
}