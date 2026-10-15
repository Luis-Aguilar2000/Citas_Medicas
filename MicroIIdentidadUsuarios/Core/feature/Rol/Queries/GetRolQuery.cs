using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using RolModel = Domain.Models.Rol;

namespace Core.feature.Rol.Queries
{
    public class GetRolQuery
        : RequestParametersGets,
          IRequest<PagedResult<RolModel>>
    {
    }

    public class GetRolQueryHandler
        : IRequestHandler<GetRolQuery, PagedResult<RolModel>>
    {
        private readonly IGenericRepository<RolModel> _repository;

        public GetRolQueryHandler(
            IGenericRepository<RolModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<RolModel>> Handle(
            GetRolQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<RolModel>(
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