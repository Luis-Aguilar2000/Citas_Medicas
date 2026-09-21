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

namespace Core.feature.Diagnostico.Queries
{
    public class GetDiagnosticoQuery
       : RequestParametersGets,
         IRequest<PagedResult<Diagnosticos>>
    {
    }

    public class GetDiagnosticoQueryHandler
       : IRequestHandler<GetDiagnosticoQuery, PagedResult<Diagnosticos>>
    {
        private readonly IGenericRepository<Diagnosticos> _repository;

        public GetDiagnosticoQueryHandler(
            IGenericRepository<Diagnosticos> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Diagnosticos>> Handle(
           GetDiagnosticoQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Diagnosticos>(request.Filter)
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
