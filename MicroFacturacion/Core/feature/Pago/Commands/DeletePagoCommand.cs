using Domain.Facturacion.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Pagos.Commands
{
    public class DeletePagoCommand : IRequest<bool>
    {
        public int IdPago { get; set; }
    }

    public class DeletePagoCommandHandler
        : IRequestHandler<DeletePagoCommand, bool>
    {
        private readonly IGenericRepository<Pago> _repository;

        public DeletePagoCommandHandler(
            IGenericRepository<Pago> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeletePagoCommand request,
            CancellationToken cancellationToken)
        {
            var pago =
                await _repository.GetByIdAsync(
                    request.IdPago);

            if (pago == null)
                return false;

            await _repository.DeleteAsync(pago);

            return true;
        }
    }
}