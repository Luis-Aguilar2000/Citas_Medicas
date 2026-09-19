namespace Domain.Facturacion.Models
{
    public class FacturaDetalle
    {
        public int IdDetalle { get; set; }

        public int IdFactura { get; set; }

        public string TipoItem { get; set; } = string.Empty;

        public int IdItem { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}