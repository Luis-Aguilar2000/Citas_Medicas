using Domain.Facturacion.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.Facturacion.Features.Servicios.Queries
{
    public class GetServiciosQuery
        : RequestParametersGets,
          IRequest<PagedResult<Servicio>>
    {
    }

    public class GetServiciosQueryHandler
        : IRequestHandler<GetServiciosQuery, PagedResult<Servicio>>
    {
        private readonly IGenericRepository<Servicio> _repository;

        public GetServiciosQueryHandler(
            IGenericRepository<Servicio> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Servicio>> Handle(
            GetServiciosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Servicio>(request.Filter)
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