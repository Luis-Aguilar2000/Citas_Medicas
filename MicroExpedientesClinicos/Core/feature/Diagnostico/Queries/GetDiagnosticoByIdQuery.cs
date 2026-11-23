using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Diagnostico.Queries
{

    public class GetDiagnosticoByIdQuery : IRequest<Diagnosticos>
    {
        public int DiagnosticoId { get; set; }
    }

    public class GetDiagnosticoByIdQueryHandler
       : IRequestHandler<GetDiagnosticoByIdQuery, Diagnosticos>
    {
        private readonly IGenericRepository<Diagnosticos> _repository;

        public GetDiagnosticoByIdQueryHandler(
            IGenericRepository<Diagnosticos> repository)
        {
            _repository = repository;
        }

        public async Task<Diagnosticos> Handle(
            GetDiagnosticoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.DiagnosticoId);
        }
    }
}
