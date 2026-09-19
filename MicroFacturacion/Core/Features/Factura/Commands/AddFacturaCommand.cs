using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Facturas.Commands
{
    public class AddFacturaCommand : IRequest<Factura>
    {
        public int IdPaciente { get; set; }

        public DateTime FechaFactura { get; set; }

        public string Estado { get; set; } = string.Empty;
    }

    public class AddFacturaCommandHandler
        : IRequestHandler<AddFacturaCommand, Factura>
    {
        private readonly IGenericRepository<Factura> _repository;

        public AddFacturaCommandHandler(
            IGenericRepository<Factura> repository)
        {
            _repository = repository;
        }

        public async Task<Factura> Handle(
            AddFacturaCommand request,
            CancellationToken cancellationToken)
        {
            var factura = new Factura
            {
                IdPaciente = request.IdPaciente,
                FechaFactura = request.FechaFactura,
                Estado = request.Estado
            };

            return await _repository.AddAsync(factura);
        }
    }
}