using Generics.Interfaces;
using MediatR;
using UsuarioModel = Domain.Models.Usuario;

namespace Core.feature.Usuario.Commands
{
    public class AddUsuarioCommand : IRequest<bool>
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool Estado { get; set; }
    }

    public class AddUsuarioCommandHandler
        : IRequestHandler<AddUsuarioCommand, bool>
    {
        private readonly IGenericRepository<UsuarioModel> _repository;

        public AddUsuarioCommandHandler(
            IGenericRepository<UsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddUsuarioCommand request,
            CancellationToken cancellationToken)
        {
            var usuario = new UsuarioModel
            {
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                Correo = request.Correo,
                PasswordHash = request.PasswordHash,
                IdRol = request.IdRol,
                Estado = request.Estado,
                FechaRegistro = DateTime.Now
            };

            await _repository.AddAsync(usuario);

            return true;
        }
    }
}