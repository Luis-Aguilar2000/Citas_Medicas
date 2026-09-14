using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AntecedentesMedicos
    {
        public int AntecedenteMedicoId { get; set; }
        public int ExpedienteId { get; set; }
        public string TipoAntecedente { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime? FechaDiagnostico { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
