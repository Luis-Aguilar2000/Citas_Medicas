using Generics.Interfaces;
using MediatR;
using RolPermisoModel = Domain.Models.RolPermiso;

namespace Core.feature.RolPermiso.Commands
{
    public class AddRolPermisoCommand : IRequest<bool>
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }
    }

    public class AddRolPermisoCommandHandler
        : IRequestHandler<AddRolPermisoCommand, bool>
    {
        private readonly IGenericRepository<RolPermisoModel> _repository;

        public AddRolPermisoCommandHandler(
            IGenericRepository<RolPermisoModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddRolPermisoCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new RolPermisoModel
            {
                IdRol = request.IdRol,
                IdPermiso = request.IdPermiso
            };

            await _repository.AddAsync(entity);

            return true;
        }
    }
}