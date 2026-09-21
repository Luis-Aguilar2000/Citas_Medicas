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

namespace Core.feature.Atencion.Queries
{

    public class GetAtencionQuery
       : RequestParametersGets,
         IRequest<PagedResult<Atenciones>>
    {
    }

    public class GetAtencionQueryHandler
       : IRequestHandler<GetAtencionQuery, PagedResult<Atenciones>>
    {
        private readonly IGenericRepository<Atenciones> _repository;

        public GetAtencionQueryHandler(
            IGenericRepository<Atenciones> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Atenciones>> Handle(
            GetAtencionQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Atenciones>(request.Filter)
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