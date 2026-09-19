using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FormasPago.Commands
{
    public class DeleteFormaPagoCommand : IRequest<bool>
    {
        public int IdFormaPago { get; set; }
    }

    public class DeleteFormaPagoCommandHandler
        : IRequestHandler<DeleteFormaPagoCommand, bool>
    {
        private readonly IGenericRepository<FormaPago> _repository;

        public DeleteFormaPagoCommandHandler(
            IGenericRepository<FormaPago> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteFormaPagoCommand request,
            CancellationToken cancellationToken)
        {
            var formaPago =
                await _repository.GetByIdAsync(
                    request.IdFormaPago);

            if (formaPago == null)
                return false;

            await _repository.DeleteAsync(formaPago);

            return true;
        }
    }
}