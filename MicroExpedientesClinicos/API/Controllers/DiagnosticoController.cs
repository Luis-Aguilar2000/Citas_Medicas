using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiagnosticoController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<Diagnosticos>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetDiagnosticoQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Diagnosticos> GetById(int id)
        {
            return await _mediator.Send(
                new GetDiagnosticoByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddDiagnosticoCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
