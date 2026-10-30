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

namespace Core.feature.Medicamentos.Queries
{
    public class GetMedicamentoQuery: RequestParametersGets,
        IRequest<PagedResult<Medicamento>>
    {
    }

    public class GetMedicamentoQueryHandler
        : IRequestHandler<GetMedicamentoQuery, PagedResult<Medicamento>>
    {
        private readonly IGenericRepository<Medicamento> _repository;

        public GetMedicamentoQueryHandler(
            IGenericRepository<Medicamento> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Medicamento>> Handle(
            GetMedicamentoQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<Medicamento>(request.Filter)
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
