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
    public class AddAntecFamiliaresCommand: IRequest<bool>
    {
        public int AntecedenteFamiliarId { get; set; }
        public int ExpedienteId { get; set; }
        public string Parentesco { get; set; } = string.Empty;
        public string Enfermedad { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }

    public class AddAntecFamiliaresCommandHandler
       : IRequestHandler<AddAntecFamiliaresCommand, bool>
    {
        private readonly IGenericRepository<AntecedentesFamiliares> _repository;

        public AddAntecFamiliaresCommandHandler(
            IGenericRepository<AntecedentesFamiliares> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddAntecFamiliaresCommand request,
            CancellationToken cancellationToken)
        {
            var antcFamiliares = new AntecedentesFamiliares
            {
               ExpedienteId = request.ExpedienteId,
               Parentesco = request.Parentesco,
               Enfermedad = request.Enfermedad,
               Observaciones = request.Observaciones

            };

            await _repository.AddAsync(antcFamiliares);

            return true;
        }
    }
}
