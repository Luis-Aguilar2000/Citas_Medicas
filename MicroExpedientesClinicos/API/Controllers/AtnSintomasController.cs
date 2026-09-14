using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtnSintomasController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<AtencionSintoma>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetAtnSintomasQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<AtencionSintoma> GetById(int id)
        {
            return await _mediator.Send(
                new GetAtnSintomasByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddAtnSintomasCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
