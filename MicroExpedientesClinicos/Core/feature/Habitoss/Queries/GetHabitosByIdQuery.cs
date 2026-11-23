using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Habitoss.Queries
{
    public class GetHabitosByIdQuery : IRequest<Habitos>
    {
        public int HabitoId { get; set; }
    }

    public class GetHabitosByIdQueryHandler
       : IRequestHandler<GetHabitosByIdQuery, Habitos>
    {
        private readonly IGenericRepository<Habitos> _repository;

        public GetHabitosByIdQueryHandler(
            IGenericRepository<Habitos> repository)
        {
            _repository = repository;
        }

        public async Task<Habitos> Handle(
            GetHabitosByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.HabitoId);
        }
    }
}
