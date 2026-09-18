using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;

namespace Core.feature.HorarioMedico.Queries
{
    public class GetHorarioMedicoQuery
        : RequestParametersGets,
          IRequest<PagedResult<HorariosMedico>>
    {
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
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<HorariosMedico>(
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