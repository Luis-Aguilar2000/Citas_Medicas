using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Sintomass.Queries
{

    public class GetSintomasByIdQuery : IRequest<Sintomas>
    {
        public int SintomaId { get; set; }
    }

    public class GetSintomasByIdQueryHandler
       : IRequestHandler<GetSintomasByIdQuery, Sintomas>
    {
        private readonly IGenericRepository<Sintomas> _repository;

        public GetSintomasByIdQueryHandler(
            IGenericRepository<Sintomas> repository)
        {
            _repository = repository;
        }

        public async Task<Sintomas> Handle(
            GetSintomasByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.SintomaId);
        }
    }
}
