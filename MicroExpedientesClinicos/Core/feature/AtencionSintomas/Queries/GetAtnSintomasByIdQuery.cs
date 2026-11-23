using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AtencionSintomas.Queries
{

    public class GetAtnSintomasByIdQuery : IRequest<AtencionSintoma>
    {
        public int AtencionSintomaId { get; set; }
    }

    public class GetAtnSintomasByIdQueryHandler
       : IRequestHandler<GetAtnSintomasByIdQuery, AtencionSintoma>
    {
        private readonly IGenericRepository<AtencionSintoma> _repository;

        public GetAtnSintomasByIdQueryHandler(
            IGenericRepository<AtencionSintoma> repository)
        {
            _repository = repository;
        }

        public async Task<AtencionSintoma> Handle(
            GetAtnSintomasByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.AtencionSintomaId);
        }
    }
}
