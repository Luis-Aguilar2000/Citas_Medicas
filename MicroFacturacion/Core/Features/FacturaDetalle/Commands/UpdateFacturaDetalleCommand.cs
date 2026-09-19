using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FacturaDetalles.Commands
{
    public class UpdateFacturaDetalleCommand : IRequest<bool>
    {
        public int IdDetalle { get; set; }

        public int IdFactura { get; set; }

        public string TipoItem { get; set; } = string.Empty;

        public int IdItem { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }

    public class UpdateFacturaDetalleCommandHandler
        : IRequestHandler<UpdateFacturaDetalleCommand, bool>
    {
        private readonly IGenericRepository<FacturaDetalle> _repository;

        public UpdateFacturaDetalleCommandHandler(
            IGenericRepository<FacturaDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateFacturaDetalleCommand request,
            CancellationToken cancellationToken)
        {
            var detalle =
                await _repository.GetByIdAsync(
                    request.IdDetalle);

            if (detalle == null)
                return false;

            detalle.IdFactura = request.IdFactura;
            detalle.TipoItem = request.TipoItem;
            detalle.IdItem = request.IdItem;
            detalle.Descripcion = request.Descripcion;
            detalle.Cantidad = request.Cantidad;
            detalle.PrecioUnitario = request.PrecioUnitario;

            await _repository.UpdateAsync(detalle);

            return true;
        }
    }
}