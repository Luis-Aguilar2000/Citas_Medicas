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
    public class UpdatePrescripCommand: IRequest<bool>
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

    public class UpdatePrescripCommandHandler
       : IRequestHandler<UpdatePrescripCommand, bool>
    {
        private readonly IGenericRepository<Prescripcion> _repository;

        public UpdatePrescripCommandHandler(
            IGenericRepository<Prescripcion> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdatePrescripCommand request,
            CancellationToken cancellationToken)
        {
            var prescripcion = await _repository.GetByIdAsync(request.IdPrescripcion);

            if (prescripcion == null)
            {
                return false;
            }

            prescripcion.IdPaciente = request.IdPaciente;
            prescripcion.IdMedico = request.IdMedico;
            prescripcion.IdCita = request.IdCita;
            prescripcion.FechaEmision = request.FechaEmision;
            prescripcion.Observaciones = request.Observaciones;
            prescripcion.Estado = request.Estado;
            prescripcion.Detalles = request.Detalles;

            await _repository.UpdateAsync(prescripcion);

            return true;
        }
    }


}
