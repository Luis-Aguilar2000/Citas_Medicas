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

    public class UpdateAtnDiagnosticosCommand : IRequest<bool>
    {
        public int AtencionDiagnosticoId { get; set; }
        public int AtencionId { get; set; }
        public int DiagnosticoId { get; set; }
        public string? Tipo { get; set; }
        public string? Observacion { get; set; }
    }

    public class UpdateAtnDiagnosticossCommandHandler
       : IRequestHandler<UpdateAtnDiagnosticosCommand, bool>
    {
        private readonly IGenericRepository<AtencionDiagnostico> _repository;

        public UpdateAtnDiagnosticosCommandHandler(
            IGenericRepository<AtencionDiagnostico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateAtnDiagnosticosCommand request,
            CancellationToken cancellationToken)
        {
            var atndiag = await _repository.GetByIdAsync(request.AtencionDiagnosticoId);

            if (atndiag == null)
            {
                return false;
            }

            atndiag.AtencionId = request.AtencionId;
            atndiag.DiagnosticoId = request.DiagnosticoId;
            atndiag.Tipo = request.Tipo;
            atndiag.Observacion = request.Observacion;


            await _repository.UpdateAsync(atndiag);

            return true;
        }
    }
}
