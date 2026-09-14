using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SigVitalesController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<SignosVitales>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetSigVitalesQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<SignosVitales> GetById(int id)
        {
            return await _mediator.Send(
                new GetSigVitalesByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddSigVitalesCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
