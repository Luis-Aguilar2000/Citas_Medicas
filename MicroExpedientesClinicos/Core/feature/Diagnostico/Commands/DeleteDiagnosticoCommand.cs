using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Diagnostico.Commands
{
   

    public class DeleteDiagnosticoCommand : IRequest<bool>
    {
        public int DiagnosticoId { get; set; }
    }

    public class DeleteDiagnosticoCommandHandler
       : IRequestHandler<DeleteDiagnosticoCommand, bool>
    {
        private readonly IGenericRepository<Diagnosticos> _repository;

        public DeleteDiagnosticoCommandHandler(
            IGenericRepository<Diagnosticos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteDiagnosticoCommand request,
            CancellationToken cancellationToken)
        {
            var diagnosticos = await _repository.GetByIdAsync(request.DiagnosticoId);

            if (diagnosticos == null)
            {
                return false;
            }

            await _repository.DeleteAsync(diagnosticos);

            return true;
        }
    }
}
