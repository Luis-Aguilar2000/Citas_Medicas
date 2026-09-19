using Domain.Facturacion.Models;
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
            var detalle =
                await _repository.GetByIdAsync(
                    request.IdDetalle);

            if (detalle == null)
                return false;

            await _repository.DeleteAsync(detalle);

            return true;
        }
    }
}