namespace Core.Features.Auth.DTOs
{
    public class UsuarioLoginDto
    {
        public int IdUsuario { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;
    }
}