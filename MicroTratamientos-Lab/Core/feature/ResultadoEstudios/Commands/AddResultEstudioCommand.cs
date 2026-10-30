using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ResultadoEstudios.Commands
{
    public class AddResultEstudioCommand: IRequest<bool>
    {
        public int IdResultadoEstudio { get; set; }
        public int IdSolicitudEstudioDetalle { get; set; }
        public int IdTecnicoOMedico { get; set; }
        public DateTime FechaResultado { get; set; }
        public string Hallazgos { get; set; }
        public string Conclusion { get; set; }
        public string ValoresReferencia { get; set; }

        public SolicitudEstudioDetalle SolicitudEstudioDetalle { get; set; }
        public List<ArchivoClinico> ArchivosClinicos { get; set; } = new();
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
                IdResultadoEstudio = request.IdResultadoEstudio,
                IdSolicitudEstudioDetalle = request.IdSolicitudEstudioDetalle,
                IdTecnicoOMedico = request.IdTecnicoOMedico,
                FechaResultado = request.FechaResultado,
                Hallazgos = request.Hallazgos,
                Conclusion = request.Conclusion,
                ValoresReferencia = request.ValoresReferencia,
                SolicitudEstudioDetalle = request.SolicitudEstudioDetalle,
                ArchivosClinicos = request.ArchivosClinicos,
                

            };

            await _repository.AddAsync(resultadoEstudio);

            return true;
        }
    }
}
