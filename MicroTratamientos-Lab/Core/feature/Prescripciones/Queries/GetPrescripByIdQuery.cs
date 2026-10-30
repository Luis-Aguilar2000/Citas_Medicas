using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Prescripciones.Queries
{
    public class GetPrescripByIdQuery: IRequest<Prescripcion>
    {
        public int IdPrescripcion { get; set; }
    }

    public class GetPrescripByIdQueryHandler
       : IRequestHandler<GetPrescripByIdQuery, Prescripcion>
    {
        private readonly IGenericRepository<Prescripcion> _repository;

        public GetPrescripByIdQueryHandler(
            IGenericRepository<Prescripcion> repository)
        {
            _repository = repository;
        }

        public async Task<Prescripcion> Handle(
            GetPrescripByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdPrescripcion);
        }
    }
}
