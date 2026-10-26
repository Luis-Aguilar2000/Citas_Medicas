using Core.Features.Auth.Interfaces;
using Generics.Interfaces;
using MediatR;
using UsuarioModel = Domain.Models.Usuario;
using RolModel = Domain.Models.Rol;

namespace Core.feature.Usuario.Commands
{
    public class AddUsuarioCommand : IRequest<bool>
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool Estado { get; set; }
    }

    public class AddUsuarioCommandHandler
        : IRequestHandler<AddUsuarioCommand, bool>
    {
        private readonly IGenericRepository<UsuarioModel> _repository;
        private readonly IGenericRepository<RolModel> _rolRepository;
        private readonly IPasswordService _passwordService;

        public AddUsuarioCommandHandler(
            IGenericRepository<UsuarioModel> repository,
            IGenericRepository<RolModel> rolRepository,
            IPasswordService passwordService)
        {
            _repository = repository;
            _rolRepository = rolRepository;
            _passwordService = passwordService;
        }

        public async Task<bool> Handle(
            AddUsuarioCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Validar nombre
            if (string.IsNullOrWhiteSpace(request.Nombres))
            {
                throw new ArgumentException(
                    "El nombre es obligatorio."
                );
            }

            // 2. Validar apellido
            if (string.IsNullOrWhiteSpace(request.Apellidos))
            {
                throw new ArgumentException(
                    "El apellido es obligatorio."
                );
            }

            // 3. Validar correo
            if (string.IsNullOrWhiteSpace(request.Correo))
            {
                throw new ArgumentException(
                    "El correo es obligatorio."
                );
            }

            // 4. Validar contraseña
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException(
                    "La contraseña es obligatoria."
                );
            }

            // 5. Verificar si ya existe el correo
            var usuarioExistente = await _repository.GetOneByAsync(
                x => x.Correo == request.Correo,
                true,
                cancellationToken
            );

            if (usuarioExistente != null)
            {
                throw new InvalidOperationException(
                    "Ya existe un usuario registrado con ese correo."
                );
            }

            // 6. Verificar que el rol exista
            var rol = await _rolRepository.GetOneByAsync(
                x => x.IdRol == request.IdRol,
                true,
                cancellationToken
            );

            if (rol == null)
            {
                throw new ArgumentException(
                    "El rol seleccionado no existe."
                );
            }

            // 7. Verificar que el rol esté activo
            if (!rol.Estado)
            {
                throw new ArgumentException(
                    "El rol seleccionado se encuentra inactivo."
                );
            }

            // 8. Generar hash de la contraseña
            var passwordHash = _passwordService.HashPassword(
                request.Password
            );

            // 9. Crear usuario
            var usuario = new UsuarioModel
            {
                Nombres = request.Nombres.Trim(),
                Apellidos = request.Apellidos.Trim(),
                Correo = request.Correo.Trim(),
                PasswordHash = passwordHash,
                IdRol = request.IdRol,
                Estado = request.Estado,
                FechaRegistro = DateTime.Now
            };

            // 10. Guardar usuario
            await _repository.AddAsync(
                usuario,
                cancellationToken
            );

            return true;
        }
    }
}