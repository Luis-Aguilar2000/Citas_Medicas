using Core.Features.Auth.DTOs;
using Core.Features.Auth.Interfaces;
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Features.Auth.Commands.Login
{
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(
            IGenericRepository<Usuario> usuarioRepository,
            IGenericRepository<Rol> rolRepository,
            IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Handle(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            // Buscar usuario por correo
            var usuario = await _usuarioRepository.GetOneByAsync(
                x => x.Correo == command.Correo,
                true,
                cancellationToken
            );

            if (usuario == null)
            {
                throw new UnauthorizedAccessException(
                    "Correo o contraseña incorrectos."
                );
            }

            // Verificar si el usuario está activo
            if (!usuario.Estado)
            {
                throw new UnauthorizedAccessException(
                    "El usuario se encuentra inactivo."
                );
            }

            // Temporal: comparar contraseña directamente
            if (usuario.PasswordHash != command.Password)
            {
                throw new UnauthorizedAccessException(
                    "Correo o contraseña incorrectos."
                );
            }

            // Obtener rol
            var rol = await _rolRepository.GetOneByAsync(
                x => x.IdRol == usuario.IdRol,
                true,
                cancellationToken
            );

            if (rol == null)
            {
                throw new Exception(
                    "El usuario no tiene un rol válido."
                );
            }

            // Token válido por 1 hora
            var expiracion = DateTime.UtcNow.AddHours(1);

            var token = _jwtService.GenerarToken(
                usuario,
                rol,
                expiracion
            );

            return new LoginResponseDto
            {
                Token = token,

                Expiracion = expiracion,

                Usuario = new UsuarioLoginDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Correo = usuario.Correo,
                    Rol = rol.Nombre
                }
            };
        }
    }
}