using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AntecMedicoController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<AntecedentesMedicos>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetAntecMedicosQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<AntecedentesMedicos> GetById(int id)
        {
            return await _mediator.Send(
                new GetAntecMedicosByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddAntecMedicosCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
