using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Facturas.Queries
{
    public class GetFacturaByIdQuery
        : IRequest<Factura?>
    {
        public int IdFactura { get; set; }
    }

    public class GetFacturaByIdQueryHandler
        : IRequestHandler<GetFacturaByIdQuery, Factura?>
    {
        private readonly IGenericRepository<Factura> _repository;

        public GetFacturaByIdQueryHandler(
            IGenericRepository<Factura> repository)
        {
            _repository = repository;
        }

        public async Task<Factura?> Handle(
            GetFacturaByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdFactura
            );
        }
    }
}