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
    public class DeleteAntecFamiliaresCommand: IRequest<bool>
    {
        public int AntecedenteFamiliarId { get; set; }
    }

    public class DeleteAntecFamiliaresCommandHandler
       : IRequestHandler<DeleteAntecFamiliaresCommand, bool>
    {
        private readonly IGenericRepository<AntecedentesFamiliares> _repository;

        public DeleteAntecFamiliaresCommandHandler(
            IGenericRepository<AntecedentesFamiliares> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteAntecFamiliaresCommand request,
            CancellationToken cancellationToken)
        {
            var antecFamiliares = await _repository.GetByIdAsync(request.AntecedenteFamiliarId);

            if (antecFamiliares == null)
            {
                return false;
            }

            await _repository.DeleteAsync(antecFamiliares);

            return true;
        }
    }
}
