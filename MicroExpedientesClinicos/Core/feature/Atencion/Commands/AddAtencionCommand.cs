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
    public class AddAtencionCommand : IRequest<bool>
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

    public class AddAtencionCommandHandler
       : IRequestHandler<AddAtencionCommand, bool>
    {
        private readonly IGenericRepository<Atenciones> _repository;

        public AddAtencionCommandHandler(
            IGenericRepository<Atenciones> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddAtencionCommand request,
            CancellationToken cancellationToken)
        {
            var atencion = new Atenciones
            {
                ExpedienteId = request.ExpedienteId,
                MedicoId = request.MedicoId,
                FechaAtencion = request.FechaAtencion,
                TipoAtencion = request.TipoAtencion,
                MotivoConsulta = request.MotivoConsulta,
                Observaciones = request.Observaciones,
                CreadoEn = request.CreadoEn,


            };

            await _repository.AddAsync(atencion);

            return true;
        }
    }
}
