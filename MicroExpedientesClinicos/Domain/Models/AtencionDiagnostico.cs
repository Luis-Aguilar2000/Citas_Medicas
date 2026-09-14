using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AtencionDiagnostico
    {
        public int AtencionDiagnosticoId { get; set; }
        public int AtencionId { get; set; }
        public int DiagnosticoId { get; set; }
        public string? Tipo { get; set; }
        public string? Observacion { get; set; }
    }
}
