using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AntecedenteMedicos.Queries
{

        public class GetAntecMedicosByIdQuery : IRequest<AntecedentesMedicos>
    {
        public int AntecedenteMedicoId { get; set; }
    }

    public class GetAntecMedicosByIdQueryHandler
       : IRequestHandler<GetAntecMedicosByIdQuery, AntecedentesMedicos>
    {
        private readonly IGenericRepository<AntecedentesMedicos> _repository;

        public GetAntecMedicosByIdQueryHandler(
            IGenericRepository<AntecedentesMedicos> repository)
        {
            _repository = repository;
        }

        public async Task<AntecedentesMedicos> Handle(
            GetAntecMedicosByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.AntecedenteMedicoId);
        }
    }
}
