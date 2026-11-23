using Core.feature.Diagnostico.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ExpedienteClinico.Commands
{
    public class UpdateExpClinicoCommand: IRequest<bool>
    {
        public int ExpedienteId { get; set; }
        public int PacienteId { get; set; }
        public string NumeroExpediente { get; set; } = string.Empty;
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }
    }

    public class UpdateExpClinicoCommandHandler
       : IRequestHandler<UpdateExpClinicoCommand, bool>
    {
        private readonly IGenericRepository<ExpedientesClinicos> _repository;

        public UpdateExpClinicoCommandHandler(
            IGenericRepository<ExpedientesClinicos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateExpClinicoCommand request,
            CancellationToken cancellationToken)
        {
            var expedientes = await _repository.GetByIdAsync(request.ExpedienteId);

            if (expedientes== null)
            {
                return false;
            }

           expedientes.PacienteId = request.PacienteId;
            expedientes.NumeroExpediente = request.NumeroExpediente;
            expedientes.FechaApertura  = request.FechaApertura;
            expedientes.Estado = request.Estado;
            expedientes.CreadoEn = request.CreadoEn;
            expedientes.ActualizadoEn = request.ActualizadoEn;


            await _repository.UpdateAsync(expedientes);

            return true;
        }
    }
}
