namespace Domain.Models
{
    public class FormaPago
    {
        public int IdFormaPago { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Estado { get; set; }
    }
}