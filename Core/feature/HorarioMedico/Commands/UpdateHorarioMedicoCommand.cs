
using Core.Services;
using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.HorarioMedico.Commands
{
    public class UpdateHorarioMedicoCommand : IRequest<bool>
    {
        public int IdHorario { get; set; }
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public int IdConsultorio { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Estado { get; set; }
    }

    public class UpdateHorarioMedicoCommandHandler
        : IRequestHandler<UpdateHorarioMedicoCommand, bool>
    {
        private readonly IGenericRepository<HorariosMedico> _repository;
        private readonly IIdentidadService _identidadService;

        public UpdateHorarioMedicoCommandHandler(
            IGenericRepository<HorariosMedico> repository,
            IIdentidadService identidadService)
        {
            _repository = repository;
            _identidadService = identidadService;
        }

        public async Task<bool> Handle(
            UpdateHorarioMedicoCommand request,
            CancellationToken cancellationToken)
        {
            var horario = await _repository.GetByIdAsync(
                request.IdHorario,
                cancellationToken);

            if (horario == null)
                return false;

            if (request.IdMedico <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un médico.");

            if (request.IdEspecialidad <= 0)
                throw new ArgumentException(
                    "Debe seleccionar una especialidad.");

            if (request.IdConsultorio <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un consultorio.");

            if (string.IsNullOrWhiteSpace(request.DiaSemana))
                throw new ArgumentException(
                    "Debe seleccionar un día de la semana.");

            if (request.HoraInicio >= request.HoraFin)
                throw new ArgumentException(
                    "La hora de inicio debe ser menor que la hora de finalización.");

            var medicoValido =
                await _identidadService.MedicoValidoAsync(
                    request.IdMedico,
                    request.IdEspecialidad,
                    cancellationToken);

            if (!medicoValido)
            {
                throw new ArgumentException(
                    "El médico no tiene asignada la especialidad seleccionada o está inactivo.");
            }

            horario.IdMedico = request.IdMedico;
            horario.IdEspecialidad = request.IdEspecialidad;
            horario.IdConsultorio = request.IdConsultorio;
            horario.DiaSemana = request.DiaSemana;
            horario.HoraInicio = request.HoraInicio;
            horario.HoraFin = request.HoraFin;
            horario.Estado = request.Estado;

            await _repository.UpdateAsync(
                horario,
                cancellationToken);

            return true;
        }
    }
}
