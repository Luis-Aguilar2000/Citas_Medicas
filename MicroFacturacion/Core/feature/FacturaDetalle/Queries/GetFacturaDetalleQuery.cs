using Domain.Facturacion.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.Facturacion.Features.FacturaDetalles.Queries
{
    public class GetFacturaDetallesQuery
        : RequestParametersGets,
          IRequest<PagedResult<FacturaDetalle>>
    {
    }

    public class GetFacturaDetallesQueryHandler
        : IRequestHandler<GetFacturaDetallesQuery, PagedResult<FacturaDetalle>>
    {
        private readonly IGenericRepository<FacturaDetalle> _repository;

        public GetFacturaDetallesQueryHandler(
            IGenericRepository<FacturaDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<FacturaDetalle>> Handle(
            GetFacturaDetallesQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<FacturaDetalle>(
                    request.Filter)
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