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

namespace Core.feature.Alergia.Queries
{
    public class GetAlergiaQuery
        : RequestParametersGets,
          IRequest<PagedResult<Alergias>>
    {
    }

    public class GetAlergiaQueryHandler
       : IRequestHandler<GetAlergiaQuery, PagedResult<Alergias>>
    {
        private readonly IGenericRepository<Alergias> _repository;

        public GetAlergiaQueryHandler(
            IGenericRepository<Alergias> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Alergias>> Handle(
            GetAlergiaQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Alergias>(request.Filter)
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
