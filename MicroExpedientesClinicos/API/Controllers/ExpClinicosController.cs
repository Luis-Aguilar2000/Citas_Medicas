using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpClinicosController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<ExpedientesClinicos>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetExpClinicoQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<ExpedientesClinicos> GetById(int id)
        {
            return await _mediator.Send(
                new GetExpClinicoByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddExpClinicoCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
