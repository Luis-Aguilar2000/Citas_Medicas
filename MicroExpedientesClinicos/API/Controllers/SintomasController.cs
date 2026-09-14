using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SintomasController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<Sintomas>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetSintomasQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Sintomas> GetById(int id)
        {
            return await _mediator.Send(
                new GetSintomasByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddSintomasCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
