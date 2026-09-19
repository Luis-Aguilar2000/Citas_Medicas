using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FormasPago.Queries
{
    public class GetFormaPagoByIdQuery
        : IRequest<FormaPago?>
    {
        public int IdFormaPago { get; set; }
    }

    public class GetFormaPagoByIdQueryHandler
        : IRequestHandler<GetFormaPagoByIdQuery, FormaPago?>
    {
        private readonly IGenericRepository<FormaPago> _repository;

        public GetFormaPagoByIdQueryHandler(
            IGenericRepository<FormaPago> repository)
        {
            _repository = repository;
        }

        public async Task<FormaPago?> Handle(
            GetFormaPagoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdFormaPago
            );
        }
    }
}