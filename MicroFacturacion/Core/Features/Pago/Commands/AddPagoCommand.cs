using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Pagos.Commands
{
    public class AddPagoCommand : IRequest<Pago>
    {
        public int IdFactura { get; set; }
        public int IdFormaPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
    }

    public class AddPagoCommandHandler : IRequestHandler<AddPagoCommand, Pago>
    {
        private readonly IGenericRepository<Pago> _pagoRepository;
        private readonly IGenericRepository<Factura> _facturaRepository;
        private readonly IGenericRepository<FacturaDetalle> _detalleRepository;
        private readonly IGenericRepository<FormaPago> _formaPagoRepository;

        public AddPagoCommandHandler(
            IGenericRepository<Pago> pagoRepository,
            IGenericRepository<Factura> facturaRepository,
            IGenericRepository<FacturaDetalle> detalleRepository,
            IGenericRepository<FormaPago> formaPagoRepository)
        {
            _pagoRepository = pagoRepository;
            _facturaRepository = facturaRepository;
            _detalleRepository = detalleRepository;
            _formaPagoRepository = formaPagoRepository;
        }

        public async Task<Pago> Handle(AddPagoCommand request, CancellationToken cancellationToken)
        {
            if (request.Monto <= 0)
                throw new ArgumentException("El monto del pago debe ser mayor que cero.");

            // 1. Validar existencia y estado de la forma de pago
            var formaPago = await _formaPagoRepository.GetOneByAsync(x => x.IdFormaPago == request.IdFormaPago, true, cancellationToken);
            if (formaPago == null)
                throw new ArgumentException("La forma de pago seleccionada no existe.");
            if (!formaPago.Estado)
                throw new ArgumentException("La forma de pago seleccionada se encuentra inactiva.");

            // 2. Buscar factura
            var factura = await _facturaRepository.GetOneByAsync(x => x.IdFactura == request.IdFactura, false, cancellationToken);
            if (factura == null)
                throw new ArgumentException("La factura seleccionada no existe.");

            // 3. Obtener detalles y calcular total
            var detalles = await _detalleRepository.GetPagedAsync(1, 1000, x => x.IdFactura == request.IdFactura, cancellationToken: cancellationToken);
            if (!detalles.Data.Any())
                throw new InvalidOperationException("La factura no tiene detalles registrados.");

            var totalFactura = detalles.Data.Sum(x => x.Cantidad * x.PrecioUnitario);
            if (totalFactura <= 0)
                throw new InvalidOperationException("El total de la factura debe ser mayor que cero.");

            // 4. Obtener pagos anteriores y calcular saldo pendiente
            var pagosAnteriores = await _pagoRepository.GetPagedAsync(1, 1000, x => x.IdFactura == request.IdFactura, cancellationToken: cancellationToken);
            var totalPagado = pagosAnteriores.Data.Sum(x => x.Monto);
            var saldoPendiente = totalFactura - totalPagado;

            if (saldoPendiente <= 0)
                throw new InvalidOperationException("La factura ya se encuentra pagada.");

            if (request.Monto > saldoPendiente)
                throw new ArgumentException($"El monto ingresado supera el saldo pendiente de ${saldoPendiente:N2}.");

            // 5. Crear y guardar el nuevo pago
            var pago = new Pago
            {
                IdFactura = request.IdFactura,
                IdFormaPago = request.IdFormaPago,
                Monto = request.Monto,
                FechaPago = request.FechaPago == default ? DateTime.Now : request.FechaPago
            };

            var pagoCreado = await _pagoRepository.AddAsync(pago, cancellationToken);

            // 6. Actualizar estado de la factura
            var nuevoTotalPagado = totalPagado + request.Monto;
            factura.Estado = nuevoTotalPagado >= totalFactura ? "PAGADA" : "PARCIAL";

            await _facturaRepository.UpdateAsync(factura, cancellationToken);

            return pagoCreado;
        }
    }
}