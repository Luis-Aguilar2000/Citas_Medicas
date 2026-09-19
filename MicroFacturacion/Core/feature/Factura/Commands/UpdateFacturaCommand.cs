using Domain.Facturacion.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Facturas.Commands
{
    public class UpdateFacturaCommand : IRequest<bool>
    {
        public int IdFactura { get; set; }

        public int IdPaciente { get; set; }

        public DateTime FechaFactura { get; set; }

        public string Estado { get; set; } = string.Empty;
    }

    public class UpdateFacturaCommandHandler
        : IRequestHandler<UpdateFacturaCommand, bool>
    {
        private readonly IGenericRepository<Factura> _repository;

        public UpdateFacturaCommandHandler(
            IGenericRepository<Factura> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateFacturaCommand request,
            CancellationToken cancellationToken)
        {
            var factura =
                await _repository.GetByIdAsync(request.IdFactura);

            if (factura == null)
                return false;

            factura.IdPaciente = request.IdPaciente;
            factura.FechaFactura = request.FechaFactura;
            factura.Estado = request.Estado;

            await _repository.UpdateAsync(factura);

            return true;
        }
    }
}