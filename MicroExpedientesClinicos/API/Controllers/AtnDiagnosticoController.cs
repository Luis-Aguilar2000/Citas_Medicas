using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtnDiagnosticosController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<AtencionDiagnostico>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetAtnDiagnosticosQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<AtencionDiagnostico> GetById(int id)
        {
            return await _mediator.Send(
                new GetAtnDiagnosticosByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddAtnDiagnosticosCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
