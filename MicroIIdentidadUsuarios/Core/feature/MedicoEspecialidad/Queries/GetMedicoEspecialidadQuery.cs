using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using MedicoEspecialidadModel = Domain.Models.MedicoEspecialidad;

namespace Core.feature.MedicoEspecialidad.Queries
{
    public class GetMedicoEspecialidadQuery
        : RequestParametersGets,
          IRequest<PagedResult<MedicoEspecialidadModel>>
    {
    }

    public class GetMedicoEspecialidadQueryHandler
        : IRequestHandler<GetMedicoEspecialidadQuery,
            PagedResult<MedicoEspecialidadModel>>
    {
        private readonly IGenericRepository<MedicoEspecialidadModel> _repository;

        public GetMedicoEspecialidadQueryHandler(
            IGenericRepository<MedicoEspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<MedicoEspecialidadModel>> Handle(
            GetMedicoEspecialidadQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<MedicoEspecialidadModel>(
                    request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                cancellationToken: cancellationToken);
        }
    }
}