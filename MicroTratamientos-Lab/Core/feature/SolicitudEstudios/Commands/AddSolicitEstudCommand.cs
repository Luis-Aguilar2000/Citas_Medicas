using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SolicitudEstudios.Commands
{
    public class AddSolicitEstudCommand: IRequest<bool>
    {
        public int IdSolicitudEstudio { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedicoSolicitante { get; set; }
        public int? IdCita { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Prioridad { get; set; }
        public string Estado { get; set; }
        public string DiagnosticoPresuntivo { get; set; }
        public List<SolicitudEstudioDetalle> Detalles { get; set; } = new();
    }

    public class AddSolicitEstudCommandHandler
        : IRequestHandler<AddSolicitEstudCommand, bool>
    {
        private readonly IGenericRepository<SolicitudEstudio> _repository;

        public AddSolicitEstudCommandHandler(
            IGenericRepository<SolicitudEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddSolicitEstudCommand request,
            CancellationToken cancellationToken)
        {
            var solicitudEstudio = new SolicitudEstudio
            {
                IdSolicitudEstudio = request.IdSolicitudEstudio,
                IdPaciente = request.IdPaciente,
                IdMedicoSolicitante = request.IdMedicoSolicitante,
                IdCita = request.IdCita,
                FechaSolicitud = request.FechaSolicitud,
                Prioridad = request.Prioridad,
                Estado = request.Estado,
                DiagnosticoPresuntivo = request.DiagnosticoPresuntivo,
                Detalles = request.Detalles,

            };

            await _repository.AddAsync(solicitudEstudio);

            return true;
        }
    }
}
