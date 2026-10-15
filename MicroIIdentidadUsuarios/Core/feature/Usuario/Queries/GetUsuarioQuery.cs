using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using UsuarioModel = Domain.Models.Usuario;

namespace Core.feature.Usuario.Queries
{
    public class GetUsuarioQuery
        : RequestParametersGets,
          IRequest<PagedResult<UsuarioModel>>
    {
    }

    public class GetUsuarioQueryHandler
        : IRequestHandler<GetUsuarioQuery, PagedResult<UsuarioModel>>
    {
        private readonly IGenericRepository<UsuarioModel> _repository;

        public GetUsuarioQueryHandler(
            IGenericRepository<UsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<UsuarioModel>> Handle(
            GetUsuarioQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<UsuarioModel>(
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