using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SignosVitales
    {
        public int SignosVitalesId { get; set; }
        public int AtencionId { get; set; }
        public int? PresionSistolica { get; set; }
        public int? PresionDiastolica { get; set; }
        public int? FrecuenciaCardiaca { get; set; }
        public int? FrecuenciaRespiratoria { get; set; }
        public decimal? Temperatura { get; set; }
        public decimal? SaturacionOxigeno { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Talla { get; set; }
        public decimal? Imc { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
