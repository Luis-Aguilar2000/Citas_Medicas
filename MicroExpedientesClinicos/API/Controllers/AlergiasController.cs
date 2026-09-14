using Core.feature.Alergia.Commands;
using Core.feature.Alergia.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlergiasController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<Alergias>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetAlergiasQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Alergias> GetById(int id)
        {
            return await _mediator.Send(
                new GetAlergiasByIdQuery
                {
                    AlergiaId = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddAlergiasCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
