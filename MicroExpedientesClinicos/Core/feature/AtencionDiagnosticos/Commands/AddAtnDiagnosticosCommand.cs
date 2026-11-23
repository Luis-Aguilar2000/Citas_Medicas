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
    public class AddAtnDiagnosticosCommand : IRequest<bool>
    {
        public int AtencionDiagnosticoId { get; set; }
        public int AtencionId { get; set; }
        public int DiagnosticoId { get; set; }
        public string? Tipo { get; set; }
        public string? Observacion { get; set; }
    }

    public class AddAtnDiagnosticosCommandHandler
       : IRequestHandler<AddAtnDiagnosticosCommand, bool>
    {
        private readonly IGenericRepository<AtencionDiagnostico> _repository;

        public AddAtnDiagnosticosCommandHandler(
            IGenericRepository<AtencionDiagnostico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddAtnDiagnosticosCommand request,
            CancellationToken cancellationToken)
        {
            var atnDiag = new AtencionDiagnostico
            {
               AtencionId = request.AtencionId,
               DiagnosticoId = request.DiagnosticoId,
               Tipo = request.Tipo,
               Observacion = request.Observacion,

            };

            await _repository.AddAsync(atnDiag);

            return true;
        }
    }
}
