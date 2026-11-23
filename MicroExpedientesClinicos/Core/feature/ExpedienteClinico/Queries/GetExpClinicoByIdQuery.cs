using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ExpedienteClinico.Queries
{

    public class GetExpClinicoByIdQuery : IRequest<ExpedientesClinicos>
    {
        public int ExpedienteId { get; set; }
    }

    public class GetExpClinicoByIdQueryHandler
       : IRequestHandler<GetExpClinicoByIdQuery, ExpedientesClinicos>
    {
        private readonly IGenericRepository<ExpedientesClinicos> _repository;

        public GetExpClinicoByIdQueryHandler(
            IGenericRepository<ExpedientesClinicos> repository)
        {
            _repository = repository;
        }

        public async Task<ExpedientesClinicos> Handle(
            GetExpClinicoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.ExpedienteId);
        }
    }
}
