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

namespace Core.feature.SignoVitales.Queries
{
    public class GetSigVitalesQuery
       : RequestParametersGets,
         IRequest<PagedResult<SignosVitales>>
    {
    }

    public class GetSigVitalesQueryHandler
       : IRequestHandler<GetSigVitalesQuery, PagedResult<SignosVitales>>
    {
        private readonly IGenericRepository<SignosVitales> _repository;

        public GetSigVitalesQueryHandler(
            IGenericRepository<SignosVitales> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<SignosVitales>> Handle(
            GetSigVitalesQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<SignosVitales>(request.Filter)
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
