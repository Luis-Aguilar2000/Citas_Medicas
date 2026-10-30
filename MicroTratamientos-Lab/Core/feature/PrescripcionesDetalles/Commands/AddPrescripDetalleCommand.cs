using Core.feature.Prescripciones.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.PrescripcionesDetalles.Commands
{
    public class AddPrescripDetalleCommand: IRequest<bool>
    {
        public int IdPrescripcionDetalle { get; set; }
        public int IdPrescripcion { get; set; }
        public int IdMedicamento { get; set; }
        public decimal Dosis { get; set; }
        public string UnidadMedida { get; set; }
        public string Frecuencia { get; set; }
        public int DuracionDias { get; set; }
        public string IndicacionesEspeciales { get; set; }
        public Prescripcion Prescripcion { get; set; }
        public Medicamento Medicamento { get; set; }
    }

    public class AddPrescripDetalleCommandHandler
      : IRequestHandler<AddPrescripDetalleCommand, bool>
    {
        private readonly IGenericRepository<PrescripcionDetalle> _repository;

        public AddPrescripDetalleCommandHandler(
            IGenericRepository<PrescripcionDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddPrescripDetalleCommand request,
            CancellationToken cancellationToken)
        {
            var prescripcionDetalle = new PrescripcionDetalle
            {
                IdPrescripcionDetalle = request.IdPrescripcionDetalle,
                IdPrescripcion = request.IdPrescripcion,
                IdMedicamento = request.IdMedicamento,
                Dosis = request.Dosis,
                UnidadMedida = request.UnidadMedida,
                Frecuencia = request.Frecuencia,
                DuracionDias = request.DuracionDias,
                IndicacionesEspeciales = request.IndicacionesEspeciales,
                Prescripcion = request.Prescripcion,
                Medicamento = request.Medicamento,

            };

            await _repository.AddAsync(prescripcionDetalle);

            return true;
        }
    }
}
