using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using SesionUsuarioModel = Domain.Models.SesionUsuario;

namespace Core.feature.SesionUsuario.Queries
{
    public class GetSesionUsuarioQuery
        : RequestParametersGets,
          IRequest<PagedResult<SesionUsuarioModel>>
    {
    }

    public class GetSesionUsuarioQueryHandler
        : IRequestHandler<GetSesionUsuarioQuery, PagedResult<SesionUsuarioModel>>
    {
        private readonly IGenericRepository<SesionUsuarioModel> _repository;

        public GetSesionUsuarioQueryHandler(
            IGenericRepository<SesionUsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<SesionUsuarioModel>> Handle(
            GetSesionUsuarioQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<SesionUsuarioModel>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                cancellationToken: cancellationToken);
        }
    }
}