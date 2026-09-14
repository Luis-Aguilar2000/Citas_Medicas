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
    public class GetAlergiasQuery: IRequest<List<Alergias>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetAlergiasQueryHandler
       : IRequestHandler<GetAlergiasQuery, List<Alergias>>
    {
        private readonly IGenericRepository<Alergias> _repository;

        public GetAlergiasQueryHandler(
            IGenericRepository<Alergias> repository)
        {
            _repository = repository;
        }

        public async Task<List<Alergias>> Handle(
            GetAlergiasQuery request,
            CancellationToken cancellationToken)
        {
            var citas = await _repository.GetAllAsync();

            return citas.ToList();
        }
    }
}
