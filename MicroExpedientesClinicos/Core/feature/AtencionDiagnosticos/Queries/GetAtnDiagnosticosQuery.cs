using Core.feature.Alergia.Queries;
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

namespace Core.feature.AtencionDiagnosticos.Queries
{
   
    public class GetAtnDiagnosticosQuery
      : RequestParametersGets,
        IRequest<PagedResult<AtencionDiagnostico>>
    {
    }

    public class GetAtnDiagnosticosQueryHandler
       : IRequestHandler<GetAtnDiagnosticosQuery, PagedResult<AtencionDiagnostico>>
    {
        private readonly IGenericRepository<AtencionDiagnostico> _repository;

        public GetAtnDiagnosticosQueryHandler(
            IGenericRepository<AtencionDiagnostico> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<AtencionDiagnostico>> Handle(
            GetAtnDiagnosticosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<AtencionDiagnostico>(request.Filter)
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
