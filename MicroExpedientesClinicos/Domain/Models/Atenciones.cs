using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Atenciones
    {
        public int AtencionId { get; set; }
        public int ExpedienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaAtencion { get; set; }
        public string TipoAtencion { get; set; } = string.Empty;
        public string MotivoConsulta { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public DateTime CreadoEn { get; set; }
    }
}
