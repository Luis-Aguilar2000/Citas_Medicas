using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Atencion.Queries
{

    public class GetAtencionByIdQuery : IRequest<Atenciones>
    {
        public int AtencionId { get; set; }
    }

    public class GetAtencionByIdQueryHandler
       : IRequestHandler<GetAtencionByIdQuery, Atenciones>
    {
        private readonly IGenericRepository<Atenciones> _repository;

        public GetAtencionByIdQueryHandler(
            IGenericRepository<Atenciones> repository)
        {
            _repository = repository;
        }

        public async Task<Atenciones> Handle(
            GetAtencionByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.AtencionId);
        }
    }
}
