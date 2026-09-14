using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AntecedentesFamiliares
    {
        public int AntecedenteFamiliarId { get; set; }
        public int ExpedienteId { get; set; }
        public string Parentesco { get; set; } = string.Empty;
        public string Enfermedad { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }
}
