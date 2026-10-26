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
        private readonly IPasswordService _passwordService;

        public LoginCommandHandler(
            IGenericRepository<Usuario> usuarioRepository,
            IGenericRepository<Rol> rolRepository,
            IJwtService jwtService,
            IPasswordService passwordService)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _jwtService = jwtService;
            _passwordService = passwordService;
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

            // Verificar si está activo
            if (!usuario.Estado)
            {
                throw new UnauthorizedAccessException(
                    "El usuario se encuentra inactivo."
                );
            }

            // Verificar contraseña contra el hash
            var passwordValido = _passwordService.VerifyPassword(
                command.Password,
                usuario.PasswordHash
            );

            if (!passwordValido)
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