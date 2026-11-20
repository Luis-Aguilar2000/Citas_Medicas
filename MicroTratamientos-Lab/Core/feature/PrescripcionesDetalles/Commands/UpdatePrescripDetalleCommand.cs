using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.PrescripcionesDetalles.Commands
{
    public class UpdatePrescripDetalleCommand : IRequest<bool>
    {
        public int IdPrescripcionDetalle { get; set; }
        public int IdPrescripcion { get; set; }
        public int IdMedicamento { get; set; }
        public decimal Dosis { get; set; }
        public string UnidadMedida { get; set; }
        public string Frecuencia { get; set; }
        public int DuracionDias { get; set; }
        public string IndicacionesEspeciales { get; set; }
    }

    public class UpdatePrescripDetalleCommandHandler
        : IRequestHandler<UpdatePrescripDetalleCommand, bool>
    {
        private readonly IGenericRepository<PrescripcionDetalle> _repository;

        public UpdatePrescripDetalleCommandHandler(
            IGenericRepository<PrescripcionDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdatePrescripDetalleCommand request,
            CancellationToken cancellationToken)
        {
            var prescripcionDetalle = await _repository.GetByIdAsync(
                request.IdPrescripcionDetalle
            );

            if (prescripcionDetalle == null)
            {
                return false;
            }

            prescripcionDetalle.IdPrescripcion = request.IdPrescripcion;
            prescripcionDetalle.IdMedicamento = request.IdMedicamento;
            prescripcionDetalle.Dosis = request.Dosis;
            prescripcionDetalle.UnidadMedida = request.UnidadMedida;
            prescripcionDetalle.Frecuencia = request.Frecuencia;
            prescripcionDetalle.DuracionDias = request.DuracionDias;
            prescripcionDetalle.IndicacionesEspeciales =
                request.IndicacionesEspeciales;

            await _repository.UpdateAsync(prescripcionDetalle);

            return true;
        }
    }
}