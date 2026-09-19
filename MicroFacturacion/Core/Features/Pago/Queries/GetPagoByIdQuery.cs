using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Pagos.Queries
{
    public class GetPagoByIdQuery : IRequest<Pago?>
    {
        public int IdPago { get; set; }
    }

    public class GetPagoByIdQueryHandler
        : IRequestHandler<GetPagoByIdQuery, Pago?>
    {
        private readonly IGenericRepository<Pago> _repository;

        public GetPagoByIdQueryHandler(
            IGenericRepository<Pago> repository)
        {
            _repository = repository;
        }

        public async Task<Pago?> Handle(
            GetPagoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdPago
            );
        }
    }
}