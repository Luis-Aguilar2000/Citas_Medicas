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
    public class DeleteMedicamentoCommand: IRequest<bool>
    {
        public int IdMedicamento { get; set; }
    }

    public class DeleteMedicamentoCommandHandler
        : IRequestHandler<DeleteMedicamentoCommand, bool>
    {
        private readonly IGenericRepository<Medicamento> _repository;

        public DeleteMedicamentoCommandHandler(
            IGenericRepository<Medicamento> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteMedicamentoCommand request,
            CancellationToken cancellationToken)
        {
            var medicamento = await _repository.GetByIdAsync(request.IdMedicamento);

            if (medicamento == null)
            {
                return false;
            }

            await _repository.DeleteAsync(medicamento);

            return true;
        }
    }
}
