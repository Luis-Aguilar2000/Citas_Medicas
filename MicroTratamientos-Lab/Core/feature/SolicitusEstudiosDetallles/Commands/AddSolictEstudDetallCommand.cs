using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.SolicitusEstudiosDetallles.Commands
{
    public class AddSolictEstudDetallCommand : IRequest<bool>
    {
        public int IdSolicitudEstudio { get; set; }
        public int IdCatalogoEstudio { get; set; }
        public string Indicaciones { get; set; }
        public string Estado { get; set; }
    }

    public class AddSolictEstudDetallCommandHandler
        : IRequestHandler<AddSolictEstudDetallCommand, bool>
    {
        private readonly IGenericRepository<SolicitudEstudioDetalle> _repository;

        public AddSolictEstudDetallCommandHandler(
            IGenericRepository<SolicitudEstudioDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddSolictEstudDetallCommand request,
            CancellationToken cancellationToken)
        {
            var solicitudEstudioDetalle = new SolicitudEstudioDetalle
            {
                IdSolicitudEstudio = request.IdSolicitudEstudio,
                IdCatalogoEstudio = request.IdCatalogoEstudio,
                Indicaciones = request.Indicaciones,
                Estado = request.Estado
            };

            await _repository.AddAsync(solicitudEstudioDetalle);

            return true;
        }
    }
}