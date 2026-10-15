using Generics.Interfaces;
using MediatR;
using PermisosModel = Domain.Models.Permisos;

namespace Core.feature.Permisos.Commands
{
    public class UpdatePermisosCommand : IRequest<bool>
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class UpdatePermisosCommandHandler
        : IRequestHandler<UpdatePermisosCommand, bool>
    {
        private readonly IGenericRepository<PermisosModel> _repository;

        public UpdatePermisosCommandHandler(
            IGenericRepository<PermisosModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdatePermisosCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdPermiso);

            if (entity == null)
                return false;

            entity.Nombre = request.Nombre;
            entity.Descripcion = request.Descripcion;
            entity.Estado = request.Estado;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}