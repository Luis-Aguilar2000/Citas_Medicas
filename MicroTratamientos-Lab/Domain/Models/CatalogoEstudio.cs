namespace Domain.Models
{
    public class CatalogoEstudio
    {
        public int IdCatalogoEstudio { get; set; }
        public string Codigo { get; set; }
        public string NombreEstudio { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }

        public decimal Precio { get; set; }

        public bool Activo { get; set; }

        public List<SolicitudEstudioDetalle> SolicitudEstudioDetalles { get; set; } = new();
    }
}