
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class HorariosMedico
    {
        [Key]
        public int IdHorario { get; set; }

        public int IdMedico { get; set; }

        public int IdEspecialidad { get; set; }

        public int IdConsultorio { get; set; }

        public string DiaSemana { get; set; } = string.Empty;

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public bool Estado { get; set; }
    }
}
