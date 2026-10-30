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
    public class UpdateResultEstudioCommand: IRequest<bool>
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

    public class UpdateResultEstudioCommandHandler
        : IRequestHandler<UpdateResultEstudioCommand, bool>
    {
        private readonly IGenericRepository<ResultadoEstudio> _repository;

        public UpdateResultEstudioCommandHandler(
            IGenericRepository<ResultadoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateResultEstudioCommand request,
            CancellationToken cancellationToken)
        {
            var resultado = await _repository.GetByIdAsync(request.IdResultadoEstudio);

            if (resultado == null)
            {
                return false;
            }

            resultado.IdResultadoEstudio = request.IdResultadoEstudio;
            resultado.IdTecnicoOMedico = request.IdTecnicoOMedico;
            resultado.FechaResultado = request.FechaResultado;
            resultado.Hallazgos = request.Hallazgos;
            resultado.Conclusion = request.Conclusion;
            resultado.ValoresReferencia = request.ValoresReferencia;
            resultado.SolicitudEstudioDetalle = request.SolicitudEstudioDetalle;
            resultado.ArchivosClinicos = request.ArchivosClinicos;
            
            
            await _repository.UpdateAsync(resultado);

            return true;
        }
    }
}
