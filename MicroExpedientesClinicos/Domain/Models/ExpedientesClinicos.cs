using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ExpedientesClinicos
    {
        public int ExpedienteId { get; set; }
        public int PacienteId { get; set; }
        public string NumeroExpediente { get; set; } = string.Empty;
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }
    }
}
