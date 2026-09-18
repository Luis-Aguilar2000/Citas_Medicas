using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.feature.Consultorios.Queries
{
    public class GetConsultorioQuery
        : RequestParametersGets,
          IRequest<PagedResult<Consultorio>>
    {
    }

    public class GetConsultorioQueryHandler
        : IRequestHandler<GetConsultorioQuery, PagedResult<Consultorio>>
    {
        private readonly IGenericRepository<Consultorio> _repository;

        public GetConsultorioQueryHandler(
            IGenericRepository<Consultorio> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Consultorio>> Handle(
            GetConsultorioQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<Consultorio>(request.Filter)
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