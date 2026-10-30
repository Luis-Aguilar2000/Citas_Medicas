using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SolicitudEstudios.Queries
{
    public class GetSolicitEstudByIdQuery: IRequest<SolicitudEstudio>
    {
        public int IdSolicitudEstudio { get; set; }
    }

    public class GetSolicitEstudByIdQueryHandler
      : IRequestHandler<GetSolicitEstudByIdQuery, SolicitudEstudio>
    {
        private readonly IGenericRepository<SolicitudEstudio> _repository;

        public GetSolicitEstudByIdQueryHandler(
            IGenericRepository<SolicitudEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<SolicitudEstudio> Handle(
            GetSolicitEstudByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdSolicitudEstudio);
        }
    }
}
