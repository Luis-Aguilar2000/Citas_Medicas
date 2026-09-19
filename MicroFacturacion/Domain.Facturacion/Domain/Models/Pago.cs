namespace Domain.Facturacion.Models
{
    public class Pago
    {
        public int IdPago { get; set; }

        public int IdFactura { get; set; }

        public int IdFormaPago { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaPago { get; set; }
    }
}