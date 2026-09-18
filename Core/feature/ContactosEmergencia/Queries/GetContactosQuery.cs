using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.feature.ContactosEmergencia.Queries
{
    public class GetContactosQuery
        : RequestParametersGets,
          IRequest<PagedResult<ContactoEmergencia>>
    {
    }

    public class GetContactosQueryHandler
        : IRequestHandler<GetContactosQuery, PagedResult<ContactoEmergencia>>
    {
        private readonly IGenericRepository<ContactoEmergencia> _repository;

        public GetContactosQueryHandler(
            IGenericRepository<ContactoEmergencia> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ContactoEmergencia>> Handle(
            GetContactosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<ContactoEmergencia>(
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