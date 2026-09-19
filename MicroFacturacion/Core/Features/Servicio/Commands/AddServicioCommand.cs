using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Servicios.Commands
{
    public class AddServicioCommand : IRequest<Servicio>
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public bool Estado { get; set; }
    }

    public class AddServicioCommandHandler
        : IRequestHandler<AddServicioCommand, Servicio>
    {
        private readonly IGenericRepository<Servicio> _repository;

        public AddServicioCommandHandler(
            IGenericRepository<Servicio> repository)
        {
            _repository = repository;
        }

        public async Task<Servicio> Handle(
            AddServicioCommand request,
            CancellationToken cancellationToken)
        {
            var servicio = new Servicio
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Precio = request.Precio,
                Estado = request.Estado
            };

            return await _repository.AddAsync(servicio);
        }
    }
}