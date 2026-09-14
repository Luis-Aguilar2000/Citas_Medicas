using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Alergia.Queries
{
    public class GetAlergiasByIdQuery: IRequest<Alergias>
    {
        public int AlergiaId { get; set; }
    }

    public class GetAlergiasByIdQueryHandler
       : IRequestHandler<GetAlergiasByIdQuery, Alergias>
    {
        private readonly IGenericRepository<Alergias> _repository;

        public GetAlergiasByIdQueryHandler(
            IGenericRepository<Alergias> repository)
        {
            _repository = repository;
        }

        public async Task<Alergias> Handle(
            GetAlergiasByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.AlergiaId);
        }
    }
}
