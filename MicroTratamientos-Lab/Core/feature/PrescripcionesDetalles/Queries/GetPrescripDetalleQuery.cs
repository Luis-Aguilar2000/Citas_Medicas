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

namespace Core.feature.PrescripcionesDetalles.Queries
{
    public class GetPrescripDetalleQuery: RequestParametersGets,
         IRequest<PagedResult<PrescripcionDetalle>>
    {
    }

    public class GetPrescripDetalleQueryHandler
       : IRequestHandler<GetPrescripDetalleQuery, PagedResult<PrescripcionDetalle>>
    {
        private readonly IGenericRepository<PrescripcionDetalle> _repository;

        public GetPrescripDetalleQueryHandler(
            IGenericRepository<PrescripcionDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<PrescripcionDetalle>> Handle(
            GetPrescripDetalleQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<PrescripcionDetalle>(request.Filter)
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
