using Generics.Interfaces;
using Generics.Models;
using Domain.Models;
using MediatR;

namespace Core.feature.ContactosEmergencia.Queries
{
    public class GetContactosQuery
        : IRequest<PagedResult<ContactoEmergencia>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
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
            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken
            );
        }
    }
}