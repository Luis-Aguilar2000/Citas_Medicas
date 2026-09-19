using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.Facturacion.Features.FormasPago.Queries
{
    public class GetFormasPagoQuery
        : RequestParametersGets,
          IRequest<PagedResult<FormaPago>>
    {
    }

    public class GetFormasPagoQueryHandler
        : IRequestHandler<GetFormasPagoQuery, PagedResult<FormaPago>>
    {
        private readonly IGenericRepository<FormaPago> _repository;

        public GetFormasPagoQueryHandler(
            IGenericRepository<FormaPago> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<FormaPago>> Handle(
            GetFormasPagoQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<FormaPago>(request.Filter)
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