using Core.feature.ArchivosClinicos.Queries;
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

namespace Core.feature.Prescripciones.Queries
{
    public class GetPrescripQuery: RequestParametersGets,
         IRequest<PagedResult<Prescripcion>>
    {
    }

    public class GetPrescripQueryHandler
        : IRequestHandler<GetPrescripQuery, PagedResult<Prescripcion>>
    {
        private readonly IGenericRepository<Prescripcion> _repository;

        public GetPrescripQueryHandler(
            IGenericRepository<Prescripcion> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Prescripcion>> Handle(
            GetPrescripQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<Prescripcion>(request.Filter)
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
