using Domain.Facturacion.Models;
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

    public class AddPagoCommandHandler
        : IRequestHandler<AddPagoCommand, Pago>
    {
        private readonly IGenericRepository<Pago> _repository;

        public AddPagoCommandHandler(
            IGenericRepository<Pago> repository)
        {
            _repository = repository;
        }

        public async Task<Pago> Handle(
            AddPagoCommand request,
            CancellationToken cancellationToken)
        {
            var pago = new Pago
            {
                IdFactura = request.IdFactura,
                IdFormaPago = request.IdFormaPago,
                Monto = request.Monto,
                FechaPago = request.FechaPago
            };

            return await _repository.AddAsync(pago);
        }
    }
}