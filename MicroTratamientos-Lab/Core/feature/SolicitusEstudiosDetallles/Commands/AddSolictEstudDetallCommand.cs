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
    public class AddSolictEstudDetallCommand: IRequest<bool>
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

    public class AddSolictEstudDetallCommandHandler
        : IRequestHandler<AddSolictEstudDetallCommand, bool>
    {
        private readonly IGenericRepository<SolicitudEstudioDetalle> _repository;

        public AddSolictEstudDetallCommandHandler(
            IGenericRepository<SolicitudEstudioDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddSolictEstudDetallCommand request,
            CancellationToken cancellationToken)
        {
            var solicitudEstudio = new SolicitudEstudioDetalle
            {
                IdSolicitudEstudioDetalle = request.IdSolicitudEstudioDetalle,
                IdSolicitudEstudio = request.IdSolicitudEstudio,
                IdCatalogoEstudio = request.IdCatalogoEstudio,
                Indicaciones = request.Indicaciones,
                Estado = request.Estado,
                SolicitudEstudio = request.SolicitudEstudio,
                CatalogoEstudio = request.CatalogoEstudio,
                ResultadoEstudio = request.ResultadoEstudio,

            };

            await _repository.AddAsync(solicitudEstudio);

            return true;
        }
    }
}
