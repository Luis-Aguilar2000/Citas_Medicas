using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Pagos.Commands
{
    public class UpdatePagoCommand : IRequest<bool>
    {
        public int IdPago { get; set; }

        public int IdFactura { get; set; }

        public int IdFormaPago { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaPago { get; set; }
    }

    public class UpdatePagoCommandHandler
        : IRequestHandler<UpdatePagoCommand, bool>
    {
        private readonly IGenericRepository<Pago> _repository;

        public UpdatePagoCommandHandler(
            IGenericRepository<Pago> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdatePagoCommand request,
            CancellationToken cancellationToken)
        {
            var pago =
                await _repository.GetByIdAsync(
                    request.IdPago);

            if (pago == null)
                return false;

            pago.IdFactura = request.IdFactura;
            pago.IdFormaPago = request.IdFormaPago;
            pago.Monto = request.Monto;
            pago.FechaPago = request.FechaPago;

            await _repository.UpdateAsync(pago);

            return true;
        }
    }
}