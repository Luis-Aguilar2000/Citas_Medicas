using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Facturas.Commands
{
    public class DeleteFacturaCommand : IRequest<bool>
    {
        public int IdFactura { get; set; }
    }

    public class DeleteFacturaCommandHandler
        : IRequestHandler<DeleteFacturaCommand, bool>
    {
        private readonly IGenericRepository<Factura> _repository;

        public DeleteFacturaCommandHandler(
            IGenericRepository<Factura> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteFacturaCommand request,
            CancellationToken cancellationToken)
        {
            var factura =
                await _repository.GetByIdAsync(
                    request.IdFactura);

            if (factura == null)
                return false;

            await _repository.DeleteAsync(factura);

            return true;
        }
    }
}