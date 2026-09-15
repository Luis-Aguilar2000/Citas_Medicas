using Generics.Interfaces;
using Generics.Models;
using Domain.Models;
using MediatR;

namespace Core.feature.Citas.Queries
{
    public class GetCitasQuery : IRequest<PagedResult<Cita>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
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
            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken
            );
        }
    }
}