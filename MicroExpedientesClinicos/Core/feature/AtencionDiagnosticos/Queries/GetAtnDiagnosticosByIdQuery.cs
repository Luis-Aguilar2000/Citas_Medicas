using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AtencionDiagnosticos.Queries
{

    public class GetAtnDiagnosticosByIdQuery : IRequest<AtencionDiagnostico>
    {
        public int AtencionDiagnosticoId { get; set; }
    }

    public class GetAtnDiagnosticosByIdQueryHandler
       : IRequestHandler<GetAtnDiagnosticosByIdQuery, AtencionDiagnostico>
    {
        private readonly IGenericRepository<AtencionDiagnostico> _repository;

        public GetAtnDiagnosticosByIdQueryHandler(
            IGenericRepository<AtencionDiagnostico> repository)
        {
            _repository = repository;
        }

        public async Task<AtencionDiagnostico> Handle(
            GetAtnDiagnosticosByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.AtencionDiagnosticoId);
        }
    }
}
