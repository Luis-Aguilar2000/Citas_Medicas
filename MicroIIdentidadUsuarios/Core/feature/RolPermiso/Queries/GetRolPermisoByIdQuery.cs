using Generics.Interfaces;
using MediatR;
using RolPermisoModel = Domain.Models.RolPermiso;

namespace Core.feature.RolPermiso.Queries
{
    public class GetRolPermisoByIdQuery : IRequest<RolPermisoModel?>
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }
    }

    public class GetRolPermisoByIdQueryHandler
        : IRequestHandler<GetRolPermisoByIdQuery, RolPermisoModel?>
    {
        private readonly IGenericRepository<RolPermisoModel> _repository;

        public GetRolPermisoByIdQueryHandler(
            IGenericRepository<RolPermisoModel> repository)
        {
            _repository = repository;
        }

        public async Task<RolPermisoModel?> Handle(
            GetRolPermisoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetOneByAsync(
                x => x.IdRol == request.IdRol &&
                     x.IdPermiso == request.IdPermiso,
                cancellationToken: cancellationToken);
        }
    }
}