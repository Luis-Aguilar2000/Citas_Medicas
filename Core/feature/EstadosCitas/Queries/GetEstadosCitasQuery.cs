using Generics.Interfaces;
using Generics.Models;
using Domain.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Queires
{
    public class GetEstadosCitasQuery
        : IRequest<PagedResult<EstadoCitas>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
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
            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken
            );
        }
    }
}