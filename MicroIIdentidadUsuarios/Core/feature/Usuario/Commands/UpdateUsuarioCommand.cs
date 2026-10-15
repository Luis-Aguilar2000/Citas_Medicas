using Generics.Interfaces;
using MediatR;
using UsuarioModel = Domain.Models.Usuario;

namespace Core.feature.Usuario.Commands
{
    public class UpdateUsuarioCommand : IRequest<bool>
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool Estado { get; set; }
    }

    public class UpdateUsuarioCommandHandler
        : IRequestHandler<UpdateUsuarioCommand, bool>
    {
        private readonly IGenericRepository<UsuarioModel> _repository;

        public UpdateUsuarioCommandHandler(
            IGenericRepository<UsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateUsuarioCommand request,
            CancellationToken cancellationToken)
        {
            var usuario =
                await _repository.GetByIdAsync(request.IdUsuario);

            if (usuario == null)
                return false;

            usuario.Nombres = request.Nombres;
            usuario.Apellidos = request.Apellidos;
            usuario.Correo = request.Correo;
            usuario.PasswordHash = request.PasswordHash;
            usuario.IdRol = request.IdRol;
            usuario.Estado = request.Estado;

            await _repository.UpdateAsync(usuario);

            return true;
        }
    }
}