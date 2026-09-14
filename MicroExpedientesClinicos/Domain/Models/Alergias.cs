using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Alergias
    {
        public int AlergiaId { get; set; }
        public int ExpedienteId { get; set; }
        public string Sustancia { get; set; } = string.Empty;
        public string? Severidad { get; set; }
        public string? Reaccion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
