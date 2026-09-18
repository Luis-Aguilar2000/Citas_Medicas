using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.feature.Citas.Queries
{
    public class GetCitasQuery
        : RequestParametersGets,
          IRequest<PagedResult<Cita>>
    {
    }

    public class GetCitasQueryHandler
        : IRequestHandler<GetCitasQuery, PagedResult<Cita>>
    {
        private readonly IGenericRepository<Cita> _repository;

        public GetCitasQueryHandler(
            IGenericRepository<Cita> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Cita>> Handle(
            GetCitasQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<Cita>(request.Filter)
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