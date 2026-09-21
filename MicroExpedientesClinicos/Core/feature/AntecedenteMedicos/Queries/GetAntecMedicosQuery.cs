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

namespace Core.feature.AntecedenteMedicos.Queries
{
    public class GetAntecMedicosQuery
       : RequestParametersGets,
         IRequest<PagedResult<AntecedentesMedicos>>
    {
    }

    public class GetAntecMedicosQueryHandler
       : IRequestHandler<GetAntecMedicosQuery, PagedResult<AntecedentesMedicos>>
    {
        private readonly IGenericRepository<AntecedentesMedicos> _repository;

        public GetAntecMedicosQueryHandler(
            IGenericRepository<AntecedentesMedicos> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<AntecedentesMedicos>> Handle(
            GetAntecMedicosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<AntecedentesMedicos>(request.Filter)
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
