using Generics.Interfaces;
using MediatR;
using RolModel = Domain.Models.Rol;

namespace Core.feature.Rol.Commands
{
    public class UpdateRolCommand : IRequest<bool>
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class UpdateRolCommandHandler
        : IRequestHandler<UpdateRolCommand, bool>
    {
        private readonly IGenericRepository<RolModel> _repository;

        public UpdateRolCommandHandler(
            IGenericRepository<RolModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateRolCommand request,
            CancellationToken cancellationToken)
        {
            var rol = await _repository.GetByIdAsync(request.IdRol);

            if (rol == null)
                return false;

            rol.Nombre = request.Nombre;
            rol.Descripcion = request.Descripcion;
            rol.Estado = request.Estado;

            await _repository.UpdateAsync(rol);

            return true;
        }
    }
}