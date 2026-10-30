using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SolicitusEstudiosDetallles.Queries
{
    public class GetSolicEstudDetallByIdQuery: IRequest<SolicitudEstudioDetalle>
    {
        public int IdSolicitudEstudioDetalle { get; set; }
    }

    public class GetSolicEstudDetallByIdQueryHandler
       : IRequestHandler<GetSolicEstudDetallByIdQuery, SolicitudEstudioDetalle>
    {
        private readonly IGenericRepository<SolicitudEstudioDetalle> _repository;

        public GetSolicEstudDetallByIdQueryHandler(
            IGenericRepository<SolicitudEstudioDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<SolicitudEstudioDetalle> Handle(
            GetSolicEstudDetallByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdSolicitudEstudioDetalle);
        }
    }
}
