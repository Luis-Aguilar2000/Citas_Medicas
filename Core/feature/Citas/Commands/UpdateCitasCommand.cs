using Core.Services;
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Citas.Commands
{
    public class UpdateCitasCommand : IRequest<bool>
    {
        public int IdCita { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public int IdConsultorio { get; set; }
        public int IdEstadoCita { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string? MotivoConsulta { get; set; }
        public string? Observaciones { get; set; }
    }

    public class UpdateCitasCommandHandler
        : IRequestHandler<UpdateCitasCommand, bool>
    {
        private readonly IGenericRepository<Cita> _repository;
        private readonly IIdentidadService _identidadService;

        public UpdateCitasCommandHandler(
            IGenericRepository<Cita> repository,
            IIdentidadService identidadService)
        {
            _repository = repository;
            _identidadService = identidadService;
        }

        public async Task<bool> Handle(
            UpdateCitasCommand request,
            CancellationToken cancellationToken)
        {
            if (request.IdMedico <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un médico.");

            if (request.IdEspecialidad <= 0)
                throw new ArgumentException(
                    "Debe seleccionar una especialidad.");

            var cita = await _repository.GetByIdAsync(
                request.IdCita,
                cancellationToken);

            if (cita == null)
                return false;

            var medicoValido =
                await _identidadService.MedicoValidoAsync(
                    request.IdMedico,
                    request.IdEspecialidad,
                    cancellationToken);

            if (!medicoValido)
            {
                throw new ArgumentException(
                    "El médico no existe, está inactivo o no tiene asignada la especialidad seleccionada.");
            }

            cita.IdPaciente = request.IdPaciente;
            cita.IdMedico = request.IdMedico;
            cita.IdEspecialidad = request.IdEspecialidad;
            cita.IdConsultorio = request.IdConsultorio;
            cita.IdEstadoCita = request.IdEstadoCita;
            cita.FechaCita = request.FechaCita;
            cita.HoraInicio = request.HoraInicio;
            cita.HoraFin = request.HoraFin;
            cita.MotivoConsulta = request.MotivoConsulta;
            cita.Observaciones = request.Observaciones;

            await _repository.UpdateAsync(
                cita,
                cancellationToken);

            return true;
        }
    }
}