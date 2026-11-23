using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.NotasMedica.Queries
{
    public class GetNotasMedByIdQuery : IRequest<NotasMedicas>
    {
        public int NotaId { get; set; }
    }

    public class GetNotasMedByIdQueryHandler
       : IRequestHandler<GetNotasMedByIdQuery, NotasMedicas>
    {
        private readonly IGenericRepository<NotasMedicas> _repository;

        public GetNotasMedByIdQueryHandler(
            IGenericRepository<NotasMedicas> repository)
        {
            _repository = repository;
        }

        public async Task<NotasMedicas> Handle(
            GetNotasMedByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.NotaId);
        }
    }
}
