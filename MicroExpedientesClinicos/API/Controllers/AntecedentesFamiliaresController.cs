using Core.feature.AntecedenteFamiliares.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AntecedentesFamiliaresController : ControllerBase
    {

        //private readonly IMediator _mediator;



        //[HttpGet]
        //public async Task<List<AntecedentesFamiliares>> Get(
        //    [FromQuery] int totalRegistros = 0)
        //{
        //    return await _mediator.Send(new GetAntecFamiliaresQuery
        //    {
        //        TotalRegistros = totalRegistros
        //    });
        //}

        //[HttpGet("{id}")]
        //public async Task<AntecedentesFamiliares> GetById(int id)
        //{
        //    return await _mediator.Send(
        //        new GetAntecFamiliaresByIdQuery
        //        {
        //            IdPaciente = id
        //        });
        //}

        //[HttpPost]
        //public async Task<bool> Post(
        //    [FromBody] AddAntecFamiliaresCommand command)
        //{
        //    return await _mediator.Send(command);
        //}

    }

}
