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
    public class UpdateSolicEstudCommand: IRequest<bool>
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

    public class UpdateSolicEstudCommandHandler
        : IRequestHandler<UpdateSolicEstudCommand, bool>
    {
        private readonly IGenericRepository<SolicitudEstudio> _repository;

        public UpdateSolicEstudCommandHandler(
            IGenericRepository<SolicitudEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateSolicEstudCommand request,
            CancellationToken cancellationToken)
        {
            var solicitud = await _repository.GetByIdAsync(request.IdSolicitudEstudio);

            if (solicitud == null)
            {
                return false;
            }

            solicitud.IdPaciente = request.IdPaciente;
            solicitud.IdMedicoSolicitante = request.IdMedicoSolicitante;
            solicitud.IdCita = request.IdCita;
            solicitud.FechaSolicitud = request.FechaSolicitud;
            solicitud.Prioridad = request.Prioridad;
            solicitud.Estado = request.Estado;
            solicitud.DiagnosticoPresuntivo = request.DiagnosticoPresuntivo;
            solicitud.Detalles = request.Detalles;

            await _repository.UpdateAsync(solicitud);

            return true;
        }
    }
}
