using Domain.Facturacion.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FormasPago.Commands
{
    public class UpdateFormaPagoCommand : IRequest<bool>
    {
        public int IdFormaPago { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Estado { get; set; }
    }

    public class UpdateFormaPagoCommandHandler
        : IRequestHandler<UpdateFormaPagoCommand, bool>
    {
        private readonly IGenericRepository<FormaPago> _repository;

        public UpdateFormaPagoCommandHandler(
            IGenericRepository<FormaPago> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateFormaPagoCommand request,
            CancellationToken cancellationToken)
        {
            var formaPago =
                await _repository.GetByIdAsync(
                    request.IdFormaPago);

            if (formaPago == null)
                return false;

            formaPago.Nombre = request.Nombre;
            formaPago.Descripcion = request.Descripcion;
            formaPago.Estado = request.Estado;

            await _repository.UpdateAsync(formaPago);

            return true;
        }
    }
}