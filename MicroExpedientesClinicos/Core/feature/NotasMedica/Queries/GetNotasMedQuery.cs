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

namespace Core.feature.NotasMedica.Queries
{
    public class GetNotasMedQuery
        : RequestParametersGets,
          IRequest<PagedResult<NotasMedicas>>
    {
    }

    public class GetNotasMedQueryHandler
       : IRequestHandler<GetNotasMedQuery, PagedResult<NotasMedicas>>
    {
        private readonly IGenericRepository<NotasMedicas> _repository;

        public GetNotasMedQueryHandler(
            IGenericRepository<NotasMedicas> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<NotasMedicas>> Handle(
            GetNotasMedQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<NotasMedicas>(request.Filter)
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
