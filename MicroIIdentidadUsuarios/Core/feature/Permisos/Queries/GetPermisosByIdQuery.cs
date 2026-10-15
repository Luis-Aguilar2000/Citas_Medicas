using Generics.Interfaces;
using MediatR;
using PermisosModel = Domain.Models.Permisos;

namespace Core.feature.Permisos.Queries
{
    public class GetPermisosByIdQuery : IRequest<PermisosModel?>
    {
        public int IdPermiso { get; set; }
    }

    public class GetPermisosByIdQueryHandler
        : IRequestHandler<GetPermisosByIdQuery, PermisosModel?>
    {
        private readonly IGenericRepository<PermisosModel> _repository;

        public GetPermisosByIdQueryHandler(
            IGenericRepository<PermisosModel> repository)
        {
            _repository = repository;
        }

        public async Task<PermisosModel?> Handle(
            GetPermisosByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdPermiso);
        }
    }
}