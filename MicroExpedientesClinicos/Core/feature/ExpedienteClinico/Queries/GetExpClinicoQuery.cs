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

namespace Core.feature.ExpedienteClinico.Queries
{
    public class GetExpClinicoQuery
       : RequestParametersGets,
         IRequest<PagedResult<ExpedientesClinicos>>
    {
    }

    public class GetExpClinicoQueryHandler
       : IRequestHandler<GetExpClinicoQuery, PagedResult<ExpedientesClinicos>>
    {
        private readonly IGenericRepository<ExpedientesClinicos> _repository;

        public GetExpClinicoQueryHandler(
            IGenericRepository<ExpedientesClinicos> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ExpedientesClinicos>> Handle(
           GetExpClinicoQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<ExpedientesClinicos>(request.Filter)
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
