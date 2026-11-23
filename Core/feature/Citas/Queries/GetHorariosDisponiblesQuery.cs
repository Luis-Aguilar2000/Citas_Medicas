
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.HorarioMedico.Queries
{
    public class GetHorariosDisponiblesQuery
        : IRequest<List<HorarioDisponibleDto>>
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public DateOnly Fecha { get; set; }
    }

    public class HorarioDisponibleDto
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public int IdConsultorio { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

    public class GetHorariosDisponiblesQueryHandler
        : IRequestHandler<
            GetHorariosDisponiblesQuery,
            List<HorarioDisponibleDto>>
    {
        private const int DuracionCitaMinutos = 30;

        private readonly IGenericRepository<HorariosMedico>
            _horarioRepository;

        private readonly IGenericRepository<Cita>
            _citaRepository;

        private readonly IGenericRepository<EstadoCitas>
            _estadoRepository;

        public GetHorariosDisponiblesQueryHandler(
            IGenericRepository<HorariosMedico> horarioRepository,
            IGenericRepository<Cita> citaRepository,
            IGenericRepository<EstadoCitas> estadoRepository)
        {
            _horarioRepository = horarioRepository;
            _citaRepository = citaRepository;
            _estadoRepository = estadoRepository;
        }

        public async Task<List<HorarioDisponibleDto>> Handle(
            GetHorariosDisponiblesQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Validar médico y especialidad.
            if (request.IdMedico <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar un médico.");
            }

            if (request.IdEspecialidad <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar una especialidad.");
            }

            // 2. Obtener el día de la semana.
            var diaSemana = ObtenerDiaSemana(request.Fecha);

            // 3. Consultar horarios.
            var horarios = await _horarioRepository.GetAllAsync(
                cancellationToken);

            var horariosDelMedico = horarios
                .Where(h =>
                    h.IdMedico == request.IdMedico &&
                    h.IdEspecialidad == request.IdEspecialidad &&
                    h.Estado &&
                    string.Equals(
                        h.DiaSemana.Trim(),
                        diaSemana,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Si no existen horarios, no hay disponibilidad.
            if (!horariosDelMedico.Any())
                return new List<HorarioDisponibleDto>();

            // 4. Consultar estados de citas.
            var estados = await _estadoRepository.GetAllAsync(
                cancellationToken);

            // Identificar estados que liberan el horario.
            var estadosCancelados = estados
                .Where(e =>
                    !string.IsNullOrWhiteSpace(e.NombreEstado) &&
                    (
                        e.NombreEstado.Equals(
                            "Cancelada",
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        e.NombreEstado.Equals(
                            "Cancelado",
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        e.NombreEstado.Equals(
                            "Anulada",
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        e.NombreEstado.Equals(
                            "Anulado",
                            StringComparison.OrdinalIgnoreCase)
                    ))
                .Select(e => e.IdEstadoCita)
                .ToHashSet();

            // 5. Consultar citas existentes.
            var citas = await _citaRepository.GetAllAsync(
                cancellationToken);

            // Excluir las citas canceladas.
            var citasDelDia = citas
                .Where(c =>
                    c.FechaCita == request.Fecha &&
                    !estadosCancelados.Contains(c.IdEstadoCita))
                .ToList();

            // 6. Generar espacios disponibles.
            var disponibles = new List<HorarioDisponibleDto>();

            foreach (var horario in horariosDelMedico)
            {
                var inicioJornada = TimeOnly.FromTimeSpan(
                    horario.HoraInicio);

                var finJornada = TimeOnly.FromTimeSpan(
                    horario.HoraFin);

                var horaActual = inicioJornada;

                // Generar espacios de 30 minutos.
                while (horaActual.AddMinutes(DuracionCitaMinutos)
                       <= finJornada)
                {
                    var horaFin = horaActual.AddMinutes(
                        DuracionCitaMinutos);

                    // Verificar si el médico o consultorio
                    // tienen otra cita en ese intervalo.
                    var ocupado = citasDelDia.Any(c =>
                        (
                            c.IdMedico == horario.IdMedico ||
                            c.IdConsultorio == horario.IdConsultorio
                        )
                        &&
                        horaActual < c.HoraFin
                        &&
                        horaFin > c.HoraInicio);

                    // Si no está ocupado, agregar disponibilidad.
                    if (!ocupado)
                    {
                        disponibles.Add(
                            new HorarioDisponibleDto
                            {
                                IdMedico = horario.IdMedico,
                                IdEspecialidad = horario.IdEspecialidad,
                                IdConsultorio = horario.IdConsultorio,
                                Fecha = request.Fecha,
                                HoraInicio = horaActual,
                                HoraFin = horaFin
                            });
                    }

                    horaActual = horaFin;
                }
            }

            // 7. Devolver horarios ordenados.
            return disponibles
                .OrderBy(x => x.HoraInicio)
                .ToList();
        }

        // Convertir fecha a día de la semana en español.
        private static string ObtenerDiaSemana(DateOnly fecha)
        {
            return fecha.DayOfWeek switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                DayOfWeek.Sunday => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
