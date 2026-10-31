namespace Core.Facturacion.Services
{
    public interface ITratamientoLabService
    {
        Task<ItemTratamientoDto?> ObtenerMedicamentoAsync(
            int idMedicamento,
            CancellationToken cancellationToken = default);

        Task<ItemTratamientoDto?> ObtenerEstudioAsync(
            int idEstudio,
            CancellationToken cancellationToken = default);
    }

    public class ItemTratamientoDto
    {
        public int IdItem { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public decimal Precio { get; set; }
    }
}