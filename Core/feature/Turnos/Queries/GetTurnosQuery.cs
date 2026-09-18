using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.feature.Turnos.Queries
{
    public class GetTurnosQuery
        : RequestParametersGets,
          IRequest<PagedResult<Turno>>
    {
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
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<Turno>(request.Filter)
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