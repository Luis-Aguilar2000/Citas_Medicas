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

    public class DeleteAntecMedicosCommand : IRequest<bool>
    {
        public int AntecedenteMedicoId { get; set; }
    }

    public class DeleteAntecMedicosCommandHandler
       : IRequestHandler<DeleteAntecMedicosCommand, bool>
    {
        private readonly IGenericRepository<AntecedentesMedicos> _repository;

        public DeleteAntecMedicosCommandHandler(
            IGenericRepository<AntecedentesMedicos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteAntecMedicosCommand request,
            CancellationToken cancellationToken)
        {
            var medico = await _repository.GetByIdAsync(request.AntecedenteMedicoId);

            if (medico == null)
            {
                return false;
            }

            await _repository.DeleteAsync(medico);

            return true;
        }
    }
}
