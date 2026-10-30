using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Prescripciones.Commands
{
    public class AddPrescripCommand: IRequest<bool>
    {
        public int IdPrescripcion { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int? IdCita { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public List<PrescripcionDetalle> Detalles { get; set; } = new();
    }

    public class AddPrescripCommandHandler
       : IRequestHandler<AddPrescripCommand, bool>
    {
        private readonly IGenericRepository<Prescripcion> _repository;

        public AddPrescripCommandHandler(
            IGenericRepository<Prescripcion> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddPrescripCommand request,
            CancellationToken cancellationToken)
        {
            var prescripcion = new Prescripcion
            {
               IdPrescripcion = request.IdPrescripcion,
               IdPaciente = request.IdPaciente,
               IdMedico = request.IdMedico,
               IdCita = request.IdCita,
               FechaEmision = request.FechaEmision,
               Observaciones = request.Observaciones,
               Estado = request.Estado,
               Detalles = request.Detalles,

            };

            await _repository.AddAsync(prescripcion);

            return true;
        }
    }
}
