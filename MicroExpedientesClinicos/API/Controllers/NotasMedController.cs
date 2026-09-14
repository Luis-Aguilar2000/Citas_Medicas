using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotasMedController : ControllerBase
    {

        private readonly IMediator _mediator;



        [HttpGet]
        public async Task<List<NotasMedicas>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetNotasMedQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<NotasMedicas> GetById(int id)
        {
            return await _mediator.Send(
                new GetNotasMedByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddNotasMedCommand command)
        {
            return await _mediator.Send(command);
        }

    }

}
