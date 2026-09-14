using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtencionController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<Atenciones>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetAtencionQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Atenciones> GetById(int id)
        {
            return await _mediator.Send(
                new GetAtencionByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddAtencionCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
