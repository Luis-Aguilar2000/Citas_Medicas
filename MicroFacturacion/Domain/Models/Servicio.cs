namespace Domain.Models
{
    public class Servicio
    {
        public int IdServicio { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public bool Estado { get; set; }
    }
}