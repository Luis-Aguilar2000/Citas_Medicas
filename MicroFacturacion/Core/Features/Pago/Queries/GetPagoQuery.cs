using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.Facturacion.Features.Pagos.Queries
{
    public class GetPagosQuery
        : RequestParametersGets,
          IRequest<PagedResult<Pago>>
    {
    }

    public class GetPagosQueryHandler
        : IRequestHandler<GetPagosQuery, PagedResult<Pago>>
    {
        private readonly IGenericRepository<Pago> _repository;

        public GetPagosQueryHandler(
            IGenericRepository<Pago> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Pago>> Handle(
            GetPagosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Pago>(
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