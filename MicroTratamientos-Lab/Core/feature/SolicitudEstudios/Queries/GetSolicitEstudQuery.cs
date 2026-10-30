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

namespace Core.feature.SolicitudEstudios.Queries
{
    public class GetSolicitEstudQuery : RequestParametersGets,
         IRequest<PagedResult<SolicitudEstudio>>
    {
    }

    public class GetSolicitEstudQueryHandler
        : IRequestHandler<GetSolicitEstudQuery, PagedResult<SolicitudEstudio>>
    {
        private readonly IGenericRepository<SolicitudEstudio> _repository;

        public GetSolicitEstudQueryHandler(
            IGenericRepository<SolicitudEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<SolicitudEstudio>> Handle(
            GetSolicitEstudQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<SolicitudEstudio>(request.Filter)
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
