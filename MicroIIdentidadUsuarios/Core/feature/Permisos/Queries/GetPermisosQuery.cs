using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using PermisosModel = Domain.Models.Permisos;

namespace Core.feature.Permisos.Queries
{
    public class GetPermisosQuery
        : RequestParametersGets,
          IRequest<PagedResult<PermisosModel>>
    {
    }

    public class GetPermisosQueryHandler
        : IRequestHandler<GetPermisosQuery, PagedResult<PermisosModel>>
    {
        private readonly IGenericRepository<PermisosModel> _repository;

        public GetPermisosQueryHandler(
            IGenericRepository<PermisosModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<PermisosModel>> Handle(
            GetPermisosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<PermisosModel>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                cancellationToken: cancellationToken);
        }
    }
}