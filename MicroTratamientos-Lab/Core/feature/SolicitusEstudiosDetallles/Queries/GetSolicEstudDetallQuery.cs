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

namespace Core.feature.SolicitusEstudiosDetallles.Queries
{
    public class GetSolicEstudDetallQuery : RequestParametersGets,
         IRequest<PagedResult<SolicitudEstudioDetalle>>
    {
    }

    public class GetSolicEstudDetallQueryHandler
        : IRequestHandler<GetSolicEstudDetallQuery, PagedResult<SolicitudEstudioDetalle>>
    {
        private readonly IGenericRepository<SolicitudEstudioDetalle> _repository;

        public GetSolicEstudDetallQueryHandler(
            IGenericRepository<SolicitudEstudioDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<SolicitudEstudioDetalle>> Handle(
            GetSolicEstudDetallQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<SolicitudEstudioDetalle>(request.Filter)
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
