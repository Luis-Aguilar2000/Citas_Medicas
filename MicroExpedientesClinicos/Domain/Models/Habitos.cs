using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Habitos
    {
        public int HabitoId { get; set; }
        public int ExpedienteId { get; set; }
        public string TipoHabito { get; set; } = string.Empty;
        public string? Frecuencia { get; set; }
        public string? Descripcion { get; set; }
    }
}
