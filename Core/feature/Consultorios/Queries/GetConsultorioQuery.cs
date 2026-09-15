using Generics.Interfaces;
using Generics.Models;
using Domain.Models;
using MediatR;

namespace Core.feature.Consultorios.Queries
{
    public class GetConsultorioQuery : IRequest<PagedResult<Consultorio>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
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
            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken
            );
        }
    }
}