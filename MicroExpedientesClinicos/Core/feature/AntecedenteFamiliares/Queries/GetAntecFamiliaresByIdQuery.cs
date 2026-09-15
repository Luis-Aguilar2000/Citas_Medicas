using Core.feature.Alergia.Queries;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AntecedenteFamiliares.Queries
{
    public class GetAntecFamiliaresByIdQuery: IRequest<AntecedentesFamiliares>
    {
        public int AntecedenteFamiliarId { get; set; }
    }

    public class GetAntecFamiliaresByIdQueryHandler
       : IRequestHandler<GetAntecFamiliaresByIdQuery, AntecedentesFamiliares>
    {
        private readonly IGenericRepository<AntecedentesFamiliares> _repository;

        public GetAntecFamiliaresByIdQueryHandler(
            IGenericRepository<AntecedentesFamiliares> repository)
        {
            _repository = repository;
        }

        public async Task<AntecedentesFamiliares> Handle(
            GetAntecFamiliaresByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.AntecedenteFamiliarId);
        }
    }
}
