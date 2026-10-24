using Domain.Models;

namespace Core.Features.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerarToken(
            Usuario usuario,
            Rol rol,
            DateTime expiracion
        );
    }
}