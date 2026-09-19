using Domain.Facturacion.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FacturaDetalles.Commands
{
    public class AddFacturaDetalleCommand : IRequest<FacturaDetalle>
    {
        public int IdFactura { get; set; }

        public string TipoItem { get; set; } = string.Empty;

        public int IdItem { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }

    public class AddFacturaDetalleCommandHandler
        : IRequestHandler<AddFacturaDetalleCommand, FacturaDetalle>
    {
        private readonly IGenericRepository<FacturaDetalle> _repository;

        public AddFacturaDetalleCommandHandler(
            IGenericRepository<FacturaDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<FacturaDetalle> Handle(
            AddFacturaDetalleCommand request,
            CancellationToken cancellationToken)
        {
            var detalle = new FacturaDetalle
            {
                IdFactura = request.IdFactura,
                TipoItem = request.TipoItem,
                IdItem = request.IdItem,
                Descripcion = request.Descripcion,
                Cantidad = request.Cantidad,
                PrecioUnitario = request.PrecioUnitario
            };

            return await _repository.AddAsync(detalle);
        }
    }
}