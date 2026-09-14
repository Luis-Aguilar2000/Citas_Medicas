using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HabitosController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<Habitos>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetHabitosQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Habitos> GetById(int id)
        {
            return await _mediator.Send(
                new GetHabitosByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddHabitosCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
