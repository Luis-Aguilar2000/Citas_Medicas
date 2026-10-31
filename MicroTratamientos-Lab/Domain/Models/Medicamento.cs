namespace Domain.Models
{
    public class Medicamento
    {
        public int IdMedicamento { get; set; }
        public string NombreComercial { get; set; }
        public string NombreGenerico { get; set; }
        public string Presentacion { get; set; }
        public string Concentracion { get; set; }
        public string ViaAdministracion { get; set; }

        public decimal Precio { get; set; }

        public bool Activo { get; set; }

        public List<PrescripcionDetalle> PrescripcionDetalles { get; set; } = new();
    }
}