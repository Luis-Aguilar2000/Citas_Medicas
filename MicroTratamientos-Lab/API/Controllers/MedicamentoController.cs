using Core.feature.CatalogoEstudios.Commands;
using Core.feature.CatalogoEstudios.Queries;
using Core.feature.Medicamentos.Commands;
using Core.feature.Medicamentos.Queries;
using Domain.Models;
using Generics.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicamentoController : ControllerBase
    {

        private readonly IMediator _mediator;

        public MedicamentoController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Medicamentos PAGINADAS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<Medicamento>>> Get(
            [FromQuery] GetMedicamentoQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Catalogo Estudio ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> GetById(int id)
        {
            var medicamento = await _mediator.Send(
                new GetMedicamentoByIdQuery
                {
                    IdMedicamento = id
                }
            );

            if (medicamento == null)
            {
                return NotFound();
            }

            return Ok(medicamento);
        }


        // =========================================
        // POST - CREAR Medicamento
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddMedicamentoCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Medicamento
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateMedicamentoCommand command)
        {
            command.IdMedicamento = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Medicamento
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteMedicamentoCommand
                {
                    IdMedicamento  = id
                }
            );

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }




    }
}
