using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FacturaDetalles.Commands
{
    public class DeleteFacturaDetalleCommand : IRequest<bool>
    {
        public int IdDetalle { get; set; }
    }

    public class DeleteFacturaDetalleCommandHandler
        : IRequestHandler<DeleteFacturaDetalleCommand, bool>
    {
        private readonly IGenericRepository<FacturaDetalle> _repository;

        public DeleteFacturaDetalleCommandHandler(
            IGenericRepository<FacturaDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteFacturaDetalleCommand request,
            CancellationToken cancellationToken)
        {
            var facturaDetalle =
                await _repository.GetByIdAsync(request.IdDetalle);

            if (facturaDetalle == null)
                return false;

            await _repository.DeleteAsync(facturaDetalle);

            return true;
        }
    }
}