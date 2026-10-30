using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Medicamentos.Commands
{
    public class UpdateMedicamentoCommand: IRequest<bool>
    {
        public int IdMedicamento { get; set; }
        public string NombreComercial { get; set; }
        public string NombreGenerico { get; set; }
        public string Presentacion { get; set; }
        public string Concentracion { get; set; }
        public string ViaAdministracion { get; set; }
        public bool Activo { get; set; }
        public List<PrescripcionDetalle> PrescripcionDetalles { get; set; } = new();
    }

    public class UpdateMedicamentoCommandHandler
        : IRequestHandler<UpdateMedicamentoCommand, bool>
    {
        private readonly IGenericRepository<Medicamento> _repository;

        public UpdateMedicamentoCommandHandler(
            IGenericRepository<Medicamento> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateMedicamentoCommand request,
            CancellationToken cancellationToken)
        {
            var medicamento = await _repository.GetByIdAsync(request.IdMedicamento);

            if (medicamento == null)
            {
                return false;
            }

            medicamento.NombreComercial = request.NombreComercial;
            medicamento.NombreGenerico = request.NombreGenerico;
            medicamento.Presentacion = request.Presentacion;
            medicamento.Concentracion = request.Concentracion;
            medicamento.ViaAdministracion = request.ViaAdministracion;
            medicamento.Activo = request.Activo;
            medicamento.PrescripcionDetalles = request.PrescripcionDetalles;

            await _repository.UpdateAsync(medicamento);

            return true;
        }
    }
}
