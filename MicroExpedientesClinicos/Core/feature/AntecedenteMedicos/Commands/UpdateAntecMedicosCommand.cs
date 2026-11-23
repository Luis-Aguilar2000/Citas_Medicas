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
    public class UpdateAntecMedicosCommand : IRequest<bool>
    {
        public int AntecedenteMedicoId { get; set; }
        public int ExpedienteId { get; set; }
        public string TipoAntecedente { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime? FechaDiagnostico { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class UpdateAntecMedicosCommandHandler
       : IRequestHandler<UpdateAntecMedicosCommand, bool>
    {
        private readonly IGenericRepository<AntecedentesMedicos> _repository;

        public UpdateAntecMedicosCommandHandler(
            IGenericRepository<AntecedentesMedicos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateAntecMedicosCommand request,
            CancellationToken cancellationToken)
        {
            var medico = await _repository.GetByIdAsync(request.AntecedenteMedicoId);

            if (medico == null)
            {
                return false;
            }

            medico.ExpedienteId = request.ExpedienteId;
            medico.TipoAntecedente = request.TipoAntecedente;
            medico.Descripcion = request.Descripcion;
            medico.FechaDiagnostico = request.FechaDiagnostico;
            medico.Estado = request.Estado;


            await _repository.UpdateAsync(medico);

            return true;
        }
    }
}
