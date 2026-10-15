using Generics.Interfaces;
using MediatR;
using RolPermisoModel = Domain.Models.RolPermiso;

namespace Core.feature.RolPermiso.Commands
{
    public class DeleteRolPermisoCommand : IRequest<bool>
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }
    }

    public class DeleteRolPermisoCommandHandler
        : IRequestHandler<DeleteRolPermisoCommand, bool>
    {
        private readonly IGenericRepository<RolPermisoModel> _repository;

        public DeleteRolPermisoCommandHandler(
            IGenericRepository<RolPermisoModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteRolPermisoCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetOneByAsync(
                x => x.IdRol == request.IdRol &&
                     x.IdPermiso == request.IdPermiso,
                cancellationToken: cancellationToken);

            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity);

            return true;
        }
    }
}