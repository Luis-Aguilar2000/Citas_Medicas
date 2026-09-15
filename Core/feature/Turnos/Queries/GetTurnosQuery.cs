using Generics.Interfaces;
using Generics.Models;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Queries
{
    public class GetTurnosQuery : IRequest<PagedResult<Turno>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class GetTurnosQueryHandler
        : IRequestHandler<GetTurnosQuery, PagedResult<Turno>>
    {
        private readonly IGenericRepository<Turno> _repository;

        public GetTurnosQueryHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Turno>> Handle(
            GetTurnosQuery request,
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