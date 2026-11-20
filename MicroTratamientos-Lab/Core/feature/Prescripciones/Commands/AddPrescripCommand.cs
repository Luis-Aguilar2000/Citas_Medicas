using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Prescripciones.Commands
{
    public class AddPrescripCommand : IRequest<int>
    {
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int? IdCita { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
    }

    public class AddPrescripCommandHandler
        : IRequestHandler<AddPrescripCommand, int>
    {
        private readonly IGenericRepository<Prescripcion> _repository;

        public AddPrescripCommandHandler(
            IGenericRepository<Prescripcion> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            AddPrescripCommand request,
            CancellationToken cancellationToken)
        {
            var prescripcion = new Prescripcion
            {
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                IdCita = request.IdCita,
                FechaEmision = request.FechaEmision,
                Observaciones = request.Observaciones,
                Estado = request.Estado
            };

            await _repository.AddAsync(prescripcion);

            return prescripcion.IdPrescripcion;
        }
    }
}