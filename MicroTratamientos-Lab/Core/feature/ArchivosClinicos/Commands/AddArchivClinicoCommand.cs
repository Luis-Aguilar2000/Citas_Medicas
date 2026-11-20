using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.ArchivosClinicos.Commands
{
    public class AddArchivClinicoCommand : IRequest<bool>
    {
        public int? IdResultadoEstudio { get; set; }
        public int? IdPaciente { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string TipoMime { get; set; }
        public string RutaAlmacenamiento { get; set; }
        public long TamanoBytes { get; set; }
        public DateTime FechaSubida { get; set; }
    }

    public class AddArchivClinicoCommandHandler
        : IRequestHandler<AddArchivClinicoCommand, bool>
    {
        private readonly IGenericRepository<ArchivoClinico> _repository;

        public AddArchivClinicoCommandHandler(
            IGenericRepository<ArchivoClinico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddArchivClinicoCommand request,
            CancellationToken cancellationToken)
        {
            var archivo = new ArchivoClinico
            {
                IdResultadoEstudio = request.IdResultadoEstudio,
                IdPaciente = request.IdPaciente,
                NombreArchivo = request.NombreArchivo,
                Extension = request.Extension,
                TipoMime = request.TipoMime,
                RutaAlmacenamiento = request.RutaAlmacenamiento,
                TamanoBytes = request.TamanoBytes,
                FechaSubida = request.FechaSubida
            };

            await _repository.AddAsync(archivo);

            return true;
        }
    }
}