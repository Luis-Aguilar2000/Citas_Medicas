using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AtencionDiagnosticos.Commands
{
    

    public class DeleteAtnDiagnosticosCommand : IRequest<bool>
    {
        public int AtencionDiagnosticoId { get; set; }
    }

    public class DeleteAtnDiagnosticosCommandHandler
       : IRequestHandler<DeleteAtnDiagnosticosCommand, bool>
    {
        private readonly IGenericRepository<AtencionDiagnostico> _repository;

        public DeleteAtnDiagnosticosCommandHandler(
            IGenericRepository<AtencionDiagnostico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteAtnDiagnosticosCommand request,
            CancellationToken cancellationToken)
        {
            var atndiag = await _repository.GetByIdAsync(request.AtencionDiagnosticoId);

            if (atndiag== null)
            {
                return false;
            }

            await _repository.DeleteAsync(atndiag);

            return true;
        }
    }
}
