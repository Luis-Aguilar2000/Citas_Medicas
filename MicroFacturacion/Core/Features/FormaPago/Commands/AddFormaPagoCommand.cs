using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FormasPago.Commands
{
    public class AddFormaPagoCommand : IRequest<FormaPago>
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Estado { get; set; }
    }

    public class AddFormaPagoCommandHandler
        : IRequestHandler<AddFormaPagoCommand, FormaPago>
    {
        private readonly IGenericRepository<FormaPago> _repository;

        public AddFormaPagoCommandHandler(
            IGenericRepository<FormaPago> repository)
        {
            _repository = repository;
        }

        public async Task<FormaPago> Handle(
            AddFormaPagoCommand request,
            CancellationToken cancellationToken)
        {
            var formaPago = new FormaPago
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Estado = request.Estado
            };

            return await _repository.AddAsync(formaPago);
        }
    }
}