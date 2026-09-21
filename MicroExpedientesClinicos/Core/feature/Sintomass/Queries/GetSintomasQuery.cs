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

namespace Core.feature.Sintomass.Queries
{
    public class GetSintomasQuery
       : RequestParametersGets,
         IRequest<PagedResult<Sintomas>>
    {
    }

    public class GetSintomasQueryHandler
       : IRequestHandler<GetSintomasQuery, PagedResult<Sintomas>>
    {
        private readonly IGenericRepository<Sintomas> _repository;

        public GetSintomasQueryHandler(
            IGenericRepository<Sintomas> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Sintomas>> Handle(
            GetSintomasQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Sintomas>(request.Filter)
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
