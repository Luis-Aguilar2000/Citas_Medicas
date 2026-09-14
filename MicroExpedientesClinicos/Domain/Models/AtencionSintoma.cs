using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AtencionSintoma
    {
        public int AtencionSintomaId { get; set; }
        public int AtencionId { get; set; }
        public int SintomaId { get; set; }
        public string? Intensidad { get; set; }
        public string? Duracion { get; set; }
    }
}
