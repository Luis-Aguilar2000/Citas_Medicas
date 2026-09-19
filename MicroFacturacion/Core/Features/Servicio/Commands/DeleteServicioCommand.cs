using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Servicios.Commands
{
    public class DeleteServicioCommand : IRequest<bool>
    {
        public int IdServicio { get; set; }
    }

    public class DeleteServicioCommandHandler
        : IRequestHandler<DeleteServicioCommand, bool>
    {
        private readonly IGenericRepository<Servicio> _repository;

        public DeleteServicioCommandHandler(
            IGenericRepository<Servicio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteServicioCommand request,
            CancellationToken cancellationToken)
        {
            var servicio =
                await _repository.GetByIdAsync(
                    request.IdServicio);

            if (servicio == null)
                return false;

            await _repository.DeleteAsync(servicio);

            return true;
        }
    }
}