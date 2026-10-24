namespace Core.Features.Auth.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiracion { get; set; }

        public UsuarioLoginDto Usuario { get; set; } = new();
    }
}