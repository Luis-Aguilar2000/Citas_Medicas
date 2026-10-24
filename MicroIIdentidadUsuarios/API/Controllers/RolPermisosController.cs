using Core.feature.RolPermiso.Commands;
using Core.feature.RolPermiso.Queries;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolPermisosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolPermisosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] RequestParametersGets request)
        {
            var result = await _mediator.Send(new GetRolPermisoQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Filter = request.Filter
            });

            return Ok(result);
        }

        [HttpGet("{idRol}/{idPermiso}")]
        public async Task<IActionResult> GetById(
            int idRol,
            int idPermiso)
        {
            var result = await _mediator.Send(
                new GetRolPermisoByIdQuery
                {
                    IdRol = idRol,
                    IdPermiso = idPermiso
                });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] AddRolPermisoCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UpdateRolPermisoCommand command)
        {
            var result = await _mediator.Send(command);

            return result ? Ok(result) : NotFound();
        }

        [HttpDelete("{idRol}/{idPermiso}")]
        public async Task<IActionResult> Delete(
            int idRol,
            int idPermiso)
        {
            var result = await _mediator.Send(
                new DeleteRolPermisoCommand
                {
                    IdRol = idRol,
                    IdPermiso = idPermiso
                });

            return result ? Ok(result) : NotFound();
        }
    }
}