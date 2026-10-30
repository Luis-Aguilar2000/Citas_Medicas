using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ResultadoEstudios.Queries
{
    public class GetResultEstudiosQuery : RequestParametersGets,
         IRequest<PagedResult<ResultadoEstudio>>
    {
    }

    public class GetResultEstudiosQueryHandler
       : IRequestHandler<GetResultEstudiosQuery, PagedResult<ResultadoEstudio>>
    {
        private readonly IGenericRepository<ResultadoEstudio> _repository;

        public GetResultEstudiosQueryHandler(
            IGenericRepository<ResultadoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ResultadoEstudio>> Handle(
            GetResultEstudiosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<ResultadoEstudio>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                filter: filter,
                cancellationToken: cancellationToken
            );
        }
    }
}
