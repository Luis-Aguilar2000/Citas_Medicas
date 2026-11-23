using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Atencion.Commands
{
  
    public class UpdateAtencionCommand : IRequest<bool>
    {
        public int AtencionId { get; set; }
        public int ExpedienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaAtencion { get; set; }
        public string TipoAtencion { get; set; } = string.Empty;
        public string MotivoConsulta { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public DateTime CreadoEn { get; set; }
    }

    public class UpdateAtencionCommandHandler
       : IRequestHandler<UpdateAtencionCommand, bool>
    {
        private readonly IGenericRepository<Atenciones> _repository;

        public UpdateAtencionCommandHandler(
            IGenericRepository<Atenciones> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateAtencionCommand request,
            CancellationToken cancellationToken)
        {
            var atencion = await _repository.GetByIdAsync(request.AtencionId);

            if (atencion == null)
            {
                return false;
            }

            atencion.ExpedienteId = request.ExpedienteId;
            atencion.MedicoId = request.MedicoId;
            atencion.FechaAtencion = request.FechaAtencion;
            atencion.TipoAtencion = request.TipoAtencion;
            atencion.MotivoConsulta = request.MotivoConsulta;
            atencion.Observaciones = request.Observaciones;
            atencion.CreadoEn = request.CreadoEn;


            await _repository.UpdateAsync(atencion);

            return true;
        }
    }
}
