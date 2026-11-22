using Core.Services;
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Citas.Commands
{
    public class AddCitasCommand : IRequest<bool>
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
        public DateTime FechaRegistro { get; set; }
    }

    public class AddCitasCommandHandler
        : IRequestHandler<AddCitasCommand, bool>
    {
        private readonly IGenericRepository<Cita> _repository;
        private readonly IIdentidadService _identidadService;

        public AddCitasCommandHandler(
            IGenericRepository<Cita> repository,
            IIdentidadService identidadService)
        {
            _repository = repository;
            _identidadService = identidadService;
        }

        public async Task<bool> Handle(
            AddCitasCommand request,
            CancellationToken cancellationToken)
        {
            if (request.IdMedico <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un médico.");

            if (request.IdEspecialidad <= 0)
                throw new ArgumentException(
                    "Debe seleccionar una especialidad.");

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

            var cita = new Cita
            {
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                IdEspecialidad = request.IdEspecialidad,
                IdConsultorio = request.IdConsultorio,
                IdEstadoCita = request.IdEstadoCita,
                FechaCita = request.FechaCita,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                MotivoConsulta = request.MotivoConsulta,
                Observaciones = request.Observaciones,
                FechaRegistro = request.FechaRegistro
            };

            await _repository.AddAsync(
                cita,
                cancellationToken);

            return true;
        }
    }
}