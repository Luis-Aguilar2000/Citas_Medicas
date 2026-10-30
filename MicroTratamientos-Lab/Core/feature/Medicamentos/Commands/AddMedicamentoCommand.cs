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
    public class AddMedicamentoCommand: IRequest<bool>
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

    public class AddMedicamentoCommandHandler
        : IRequestHandler<AddMedicamentoCommand, bool>
    {
        private readonly IGenericRepository<Medicamento> _repository;

        public AddMedicamentoCommandHandler(
            IGenericRepository<Medicamento> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddMedicamentoCommand request,
            CancellationToken cancellationToken)
        {
            var medicamento = new Medicamento
            {
                IdMedicamento = request.IdMedicamento,
                NombreComercial = request.NombreComercial,
                NombreGenerico = request.NombreGenerico,
                Presentacion = request.Presentacion,
                Concentracion = request.Concentracion,
                ViaAdministracion = request.ViaAdministracion,
                Activo = request.Activo,
                PrescripcionDetalles = request.PrescripcionDetalles,

            };

            await _repository.AddAsync(medicamento);

            return true;
        }
    }
}
