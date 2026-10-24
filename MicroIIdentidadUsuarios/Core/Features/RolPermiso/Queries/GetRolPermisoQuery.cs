using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using RolPermisoModel = Domain.Models.RolPermiso;

namespace Core.feature.RolPermiso.Queries
{
    public class GetRolPermisoQuery
        : RequestParametersGets,
          IRequest<PagedResult<RolPermisoModel>>
    {
    }

    public class GetRolPermisoQueryHandler
        : IRequestHandler<GetRolPermisoQuery, PagedResult<RolPermisoModel>>
    {
        private readonly IGenericRepository<RolPermisoModel> _repository;

        public GetRolPermisoQueryHandler(
            IGenericRepository<RolPermisoModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<RolPermisoModel>> Handle(
            GetRolPermisoQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<RolPermisoModel>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                cancellationToken: cancellationToken);
        }
    }
}