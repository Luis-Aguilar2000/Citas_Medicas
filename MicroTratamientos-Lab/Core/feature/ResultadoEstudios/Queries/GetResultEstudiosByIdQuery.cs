using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ResultadoEstudios.Queries
{
    public class GetResultEstudiosByIdQuery: IRequest<ResultadoEstudio>
    {
        public int IdResultadoEstudio { get; set; }
    }

    public class GetResultEstudiosByIdQueryHandler
      : IRequestHandler<GetResultEstudiosByIdQuery, ResultadoEstudio>
    {
        private readonly IGenericRepository<ResultadoEstudio> _repository;

        public GetResultEstudiosByIdQueryHandler(
            IGenericRepository<ResultadoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<ResultadoEstudio> Handle(
            GetResultEstudiosByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdResultadoEstudio);
        }
    }
}
