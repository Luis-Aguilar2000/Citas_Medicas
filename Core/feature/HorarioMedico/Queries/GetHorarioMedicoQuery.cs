using Generics.Interfaces;
using Generics.Models;
using Domain.Models;
using MediatR;

namespace Core.feature.HorarioMedico.Queries
{
    public class GetHorarioMedicoQuery
        : IRequest<PagedResult<HorariosMedico>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class GetHorarioMedicoQueryHandler
        : IRequestHandler<GetHorarioMedicoQuery, PagedResult<HorariosMedico>>
    {
        private readonly IGenericRepository<HorariosMedico> _repository;

        public GetHorarioMedicoQueryHandler(
            IGenericRepository<HorariosMedico> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<HorariosMedico>> Handle(
            GetHorarioMedicoQuery request,
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