using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.ResultadoEstudios.Commands
{
    public class AddResultEstudioCommand : IRequest<bool>
    {
        public int IdSolicitudEstudioDetalle { get; set; }
        public int IdTecnicoOMedico { get; set; }
        public DateTime FechaResultado { get; set; }
        public string Hallazgos { get; set; }
        public string Conclusion { get; set; }
        public string ValoresReferencia { get; set; }
    }

    public class AddResultEstudioCommandHandler
        : IRequestHandler<AddResultEstudioCommand, bool>
    {
        private readonly IGenericRepository<ResultadoEstudio> _repository;

        public AddResultEstudioCommandHandler(
            IGenericRepository<ResultadoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddResultEstudioCommand request,
            CancellationToken cancellationToken)
        {
            var resultadoEstudio = new ResultadoEstudio
            {
                IdSolicitudEstudioDetalle = request.IdSolicitudEstudioDetalle,
                IdTecnicoOMedico = request.IdTecnicoOMedico,
                FechaResultado = request.FechaResultado,
                Hallazgos = request.Hallazgos,
                Conclusion = request.Conclusion,
                ValoresReferencia = request.ValoresReferencia
            };

            await _repository.AddAsync(resultadoEstudio);

            return true;
        }
    }
}