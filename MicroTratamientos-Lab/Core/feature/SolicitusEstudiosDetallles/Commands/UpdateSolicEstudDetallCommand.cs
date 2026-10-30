using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SolicitusEstudiosDetallles.Commands
{
    public  class UpdateSolicEstudDetallCommand: IRequest<bool>
    {
        public int IdSolicitudEstudioDetalle { get; set; }
        public int IdSolicitudEstudio { get; set; }
        public int IdCatalogoEstudio { get; set; }
        public string Indicaciones { get; set; }
        public string Estado { get; set; }
        public SolicitudEstudio SolicitudEstudio { get; set; }
        public CatalogoEstudio CatalogoEstudio { get; set; }
        public ResultadoEstudio ResultadoEstudio { get; set; }
    }

    public class UpdateSolicEstudDetallCommandHandler
       : IRequestHandler<UpdateSolicEstudDetallCommand, bool>
    {
        private readonly IGenericRepository<SolicitudEstudioDetalle> _repository;

        public UpdateSolicEstudDetallCommandHandler(
            IGenericRepository<SolicitudEstudioDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateSolicEstudDetallCommand request,
            CancellationToken cancellationToken)
        {
            var solicitudEstudioDetalle = await _repository.GetByIdAsync(request.IdSolicitudEstudioDetalle);

            if (solicitudEstudioDetalle == null)
            {
                return false;
            }

            solicitudEstudioDetalle.IdSolicitudEstudio = request.IdSolicitudEstudio;
            solicitudEstudioDetalle.IdCatalogoEstudio = request.IdCatalogoEstudio;
            solicitudEstudioDetalle.Indicaciones = request.Indicaciones;
            solicitudEstudioDetalle.Estado = request.Estado;
            solicitudEstudioDetalle.SolicitudEstudio = request.SolicitudEstudio;
            solicitudEstudioDetalle.CatalogoEstudio = request.CatalogoEstudio;
            solicitudEstudioDetalle.ResultadoEstudio= request.ResultadoEstudio;

            await _repository.UpdateAsync(solicitudEstudioDetalle);

            return true;
        }
    }
}
