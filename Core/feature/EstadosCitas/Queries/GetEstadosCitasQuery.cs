using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Queires
{
    public class GetEstadosCitasQuery
        : RequestParametersGets,
          IRequest<PagedResult<EstadoCitas>>
    {
    }

    public class GetEstadosCitasQueryHandler
        : IRequestHandler<GetEstadosCitasQuery, PagedResult<EstadoCitas>>
    {
        private readonly IGenericRepository<EstadoCitas> _repository;

        public GetEstadosCitasQueryHandler(
            IGenericRepository<EstadoCitas> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<EstadoCitas>> Handle(
            GetEstadosCitasQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<EstadoCitas>(request.Filter)
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