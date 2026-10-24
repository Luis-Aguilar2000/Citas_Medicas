using Core.Features.Auth.DTOs;
using MediatR;

namespace Core.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Correo { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}