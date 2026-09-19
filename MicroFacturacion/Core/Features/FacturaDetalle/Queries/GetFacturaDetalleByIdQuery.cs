using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FacturaDetalles.Queries
{
    public class GetFacturaDetalleByIdQuery
        : IRequest<FacturaDetalle?>
    {
        public int IdDetalle { get; set; }
    }

    public class GetFacturaDetalleByIdQueryHandler
        : IRequestHandler<GetFacturaDetalleByIdQuery, FacturaDetalle?>
    {
        private readonly IGenericRepository<FacturaDetalle> _repository;

        public GetFacturaDetalleByIdQueryHandler(
            IGenericRepository<FacturaDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<FacturaDetalle?> Handle(
            GetFacturaDetalleByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdDetalle
            );
        }
    }
}