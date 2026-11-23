using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SignoVitales.Queries
{

    public class GetSigVitalesByIdQuery : IRequest<SignosVitales>
    {
        public int SignosVitalesId { get; set; }
    }

    public class GetSigVitalesByIdQueryHandler
       : IRequestHandler<GetSigVitalesByIdQuery, SignosVitales>
    {
        private readonly IGenericRepository<SignosVitales> _repository;

        public GetSigVitalesByIdQueryHandler(
            IGenericRepository<SignosVitales> repository)
        {
            _repository = repository;
        }

        public async Task<SignosVitales> Handle(
            GetSigVitalesByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.SignosVitalesId);
        }
    }
}
