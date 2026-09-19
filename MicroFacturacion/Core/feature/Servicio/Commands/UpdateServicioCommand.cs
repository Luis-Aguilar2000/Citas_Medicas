using Domain.Facturacion.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Servicios.Commands
{
    public class UpdateServicioCommand : IRequest<bool>
    {
        public int IdServicio { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public bool Estado { get; set; }
    }

    public class UpdateServicioCommandHandler
        : IRequestHandler<UpdateServicioCommand, bool>
    {
        private readonly IGenericRepository<Servicio> _repository;

        public UpdateServicioCommandHandler(
            IGenericRepository<Servicio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateServicioCommand request,
            CancellationToken cancellationToken)
        {
            var servicio =
                await _repository.GetByIdAsync(
                    request.IdServicio);

            if (servicio == null)
                return false;

            servicio.Nombre = request.Nombre;
            servicio.Descripcion = request.Descripcion;
            servicio.Precio = request.Precio;
            servicio.Estado = request.Estado;

            await _repository.UpdateAsync(servicio);

            return true;
        }
    }
}