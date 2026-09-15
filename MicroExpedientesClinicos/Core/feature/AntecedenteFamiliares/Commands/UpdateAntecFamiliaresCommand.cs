using Core.feature.Alergia.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AntecedenteFamiliares.Commands
{
    public class UpdateAntecFamiliaresCommand: IRequest<bool>
    {
        public int AntecedenteFamiliarId { get; set; }
        public int ExpedienteId { get; set; }
        public string Parentesco { get; set; } = string.Empty;
        public string Enfermedad { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }

    public class UpdateAntecFamiliaresCommandHandler
       : IRequestHandler<UpdateAntecFamiliaresCommand, bool>
    {
        private readonly IGenericRepository<AntecedentesFamiliares> _repository;

        public UpdateAntecFamiliaresCommandHandler(
            IGenericRepository<AntecedentesFamiliares> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateAntecFamiliaresCommand request,
            CancellationToken cancellationToken)
        {
            var antecFamiliares = await _repository.GetByIdAsync(request.AntecedenteFamiliarId);

            if (antecFamiliares == null)
            {
                return false;
            }

            antecFamiliares.ExpedienteId = request.ExpedienteId;
            antecFamiliares.Parentesco = request.Parentesco;
            antecFamiliares.Enfermedad = request.Enfermedad;
            antecFamiliares.Observaciones = request.Observaciones;
            

            await _repository.UpdateAsync(antecFamiliares);

            return true;
        }
    }
}
