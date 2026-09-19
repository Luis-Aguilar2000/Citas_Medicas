using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Servicios.Queries
{
    public class GetServicioByIdQuery
        : IRequest<Servicio?>
    {
        public int IdServicio { get; set; }
    }

    public class GetServicioByIdQueryHandler
        : IRequestHandler<GetServicioByIdQuery, Servicio?>
    {
        private readonly IGenericRepository<Servicio> _repository;

        public GetServicioByIdQueryHandler(
            IGenericRepository<Servicio> repository)
        {
            _repository = repository;
        }

        public async Task<Servicio?> Handle(
            GetServicioByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdServicio
            );
        }
    }
}