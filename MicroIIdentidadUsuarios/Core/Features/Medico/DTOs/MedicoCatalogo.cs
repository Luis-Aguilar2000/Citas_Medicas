
namespace Core.feature.Medico.DTOs
{
    public class MedicoCatalogoDto
    {
        public int IdMedico { get; set; }

        public int IdUsuario { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string NumeroJVPM { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}
