
using Core.Services;
using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.HorarioMedico.Commands
{
    public class AddHorarioMedicoCommand : IRequest<bool>
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

    public class AddHorarioMedicoCommandHandler
        : IRequestHandler<AddHorarioMedicoCommand, bool>
    {
        private readonly IGenericRepository<HorariosMedico> _repository;
        private readonly IIdentidadService _identidadService;

        public AddHorarioMedicoCommandHandler(
            IGenericRepository<HorariosMedico> repository,
            IIdentidadService identidadService)
        {
            _repository = repository;
            _identidadService = identidadService;
        }

        public async Task<bool> Handle(
            AddHorarioMedicoCommand request,
            CancellationToken cancellationToken)
        {
            if (request.IdMedico <= 0)
                throw new ArgumentException("Debe seleccionar un médico.");

            if (request.IdEspecialidad <= 0)
                throw new ArgumentException("Debe seleccionar una especialidad.");

            if (request.IdConsultorio <= 0)
                throw new ArgumentException("Debe seleccionar un consultorio.");

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

            var horario = new HorariosMedico
            {
                IdMedico = request.IdMedico,
                IdEspecialidad = request.IdEspecialidad,
                IdConsultorio = request.IdConsultorio,
                DiaSemana = request.DiaSemana,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                Estado = request.Estado
            };

            await _repository.AddAsync(
                horario,
                cancellationToken);

            return true;
        }
    }
}
