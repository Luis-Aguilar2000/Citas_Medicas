using Domain.Facturacion.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.Facturacion.Features.Facturas.Queries
{
    public class GetFacturasQuery
        : RequestParametersGets,
          IRequest<PagedResult<Factura>>
    {
    }

    public class GetFacturasQueryHandler
        : IRequestHandler<GetFacturasQuery, PagedResult<Factura>>
    {
        private readonly IGenericRepository<Factura> _repository;

        public GetFacturasQueryHandler(
            IGenericRepository<Factura> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Factura>> Handle(
            GetFacturasQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Factura>(request.Filter)
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