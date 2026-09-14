using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AntecedentesFamiliaresController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<Alergias>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetPacienteQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Alergias> GetById(int id)
        {
            return await _mediator.Send(
                new GetPacienteByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddPacienteCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
