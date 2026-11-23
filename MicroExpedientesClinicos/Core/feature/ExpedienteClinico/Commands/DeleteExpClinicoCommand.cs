using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ExpedienteClinico.Commands
{ 

    public class DeleteExpClinicoCommand : IRequest<bool>
    {
        public int ExpedienteId { get; set; }
    }

    public class DeleteExpClinicoCommandHandler
       : IRequestHandler<DeleteExpClinicoCommand, bool>
    {
        private readonly IGenericRepository<ExpedientesClinicos> _repository;

        public DeleteExpClinicoCommandHandler(
            IGenericRepository<ExpedientesClinicos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteExpClinicoCommand request,
            CancellationToken cancellationToken)
        {
            var expedientes = await _repository.GetByIdAsync(request.ExpedienteId);

            if (expedientes == null)
            {
                return false;
            }

            await _repository.DeleteAsync(expedientes);

            return true;
        }
    }
}
