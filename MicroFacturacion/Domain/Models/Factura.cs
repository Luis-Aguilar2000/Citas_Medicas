namespace Domain.Models
{
    public class Factura
    {
        public int IdFactura { get; set; }

        public int IdPaciente { get; set; }

        public DateTime FechaFactura { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}