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

namespace Core.feature.AntecedenteFamiliares.Queries
{
   
    public class GetAntecFamiliaresQuery
        : RequestParametersGets,
          IRequest<PagedResult<AntecedentesFamiliares>>
    {
    }

    public class GetAntecFamiliaresQueryHandler
       : IRequestHandler<GetAntecFamiliaresQuery, PagedResult<AntecedentesFamiliares>>
    {
        private readonly IGenericRepository<AntecedentesFamiliares> _repository;

        public GetAntecFamiliaresQueryHandler(
            IGenericRepository<AntecedentesFamiliares> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<AntecedentesFamiliares>> Handle(
            GetAntecFamiliaresQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<AntecedentesFamiliares>(request.Filter)
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
