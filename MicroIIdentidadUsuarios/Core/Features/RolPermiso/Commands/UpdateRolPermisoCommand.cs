using Generics.Interfaces;
using MediatR;
using RolPermisoModel = Domain.Models.RolPermiso;

namespace Core.feature.RolPermiso.Commands
{
    public class UpdateRolPermisoCommand : IRequest<bool>
    {
        public int IdRolActual { get; set; }
        public int IdPermisoActual { get; set; }

        public int IdRol { get; set; }
        public int IdPermiso { get; set; }
    }

    public class UpdateRolPermisoCommandHandler
        : IRequestHandler<UpdateRolPermisoCommand, bool>
    {
        private readonly IGenericRepository<RolPermisoModel> _repository;

        public UpdateRolPermisoCommandHandler(
            IGenericRepository<RolPermisoModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateRolPermisoCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetOneByAsync(
                x => x.IdRol == request.IdRolActual &&
                     x.IdPermiso == request.IdPermisoActual,
                asNoTracking: false,
                cancellationToken: cancellationToken);

            if (entity == null)
                return false;

            entity.IdRol = request.IdRol;
            entity.IdPermiso = request.IdPermiso;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}