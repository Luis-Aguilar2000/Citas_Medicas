using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AntecedenteMedicos.Commands
{
   
    public class AddAntecMedicosCommand : IRequest<bool>
    {
        public int AntecedenteMedicoId { get; set; }
        public int ExpedienteId { get; set; }
        public string TipoAntecedente { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime? FechaDiagnostico { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class AddAntecMedicosCommandHandler
       : IRequestHandler<AddAntecMedicosCommand, bool>
    {
        private readonly IGenericRepository<AntecedentesMedicos> _repository;

        public AddAntecMedicosCommandHandler(
            IGenericRepository<AntecedentesMedicos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddAntecMedicosCommand request,
            CancellationToken cancellationToken)
        {
            var Antecmedico = new AntecedentesMedicos
            {
               ExpedienteId = request.ExpedienteId,
               TipoAntecedente = request.TipoAntecedente,
               Descripcion = request.Descripcion,
               FechaDiagnostico = request.FechaDiagnostico,
               Estado = request.Estado


            };

            await _repository.AddAsync(Antecmedico);

            return true;
        }
    }
}
