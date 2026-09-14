using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class NotasMedicas
    {
        public int NotaId { get; set; }
        public int AtencionId { get; set; }
        public string? Subjetivo { get; set; }
        public string? Objetivo { get; set; }
        public string? Analisis { get; set; }
        public string? Plan { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
