using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.PrescripcionesDetalles.Queries
{
    public class GetPrescripDetalleByIdQuery: IRequest<PrescripcionDetalle>
    {
        public int IdPrescripcionDetalle { get; set; }
    }

    public class GetPrescripDetalleByIdQueryHandler
      : IRequestHandler<GetPrescripDetalleByIdQuery, PrescripcionDetalle>
    {
        private readonly IGenericRepository<PrescripcionDetalle> _repository;

        public GetPrescripDetalleByIdQueryHandler(
            IGenericRepository<PrescripcionDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<PrescripcionDetalle> Handle(
            GetPrescripDetalleByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdPrescripcionDetalle);
        }
    }
}
