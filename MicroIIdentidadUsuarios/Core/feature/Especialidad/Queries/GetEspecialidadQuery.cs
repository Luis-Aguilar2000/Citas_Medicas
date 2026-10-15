using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using EspecialidadModel = Domain.Models.Especialidad;

namespace Core.feature.Especialidad.Queries
{
    public class GetEspecialidadQuery
        : RequestParametersGets,
          IRequest<PagedResult<EspecialidadModel>>
    {
    }

    public class GetEspecialidadQueryHandler
        : IRequestHandler<GetEspecialidadQuery, PagedResult<EspecialidadModel>>
    {
        private readonly IGenericRepository<EspecialidadModel> _repository;

        public GetEspecialidadQueryHandler(
            IGenericRepository<EspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<EspecialidadModel>> Handle(
            GetEspecialidadQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<EspecialidadModel>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                cancellationToken: cancellationToken);
        }
    }
}