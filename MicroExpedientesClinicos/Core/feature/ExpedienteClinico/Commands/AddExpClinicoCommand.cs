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
    public class AddExpClinicoCommand : IRequest<bool>
    {
        public int ExpedienteId { get; set; }
        public int PacienteId { get; set; }
        public string NumeroExpediente { get; set; } = string.Empty;
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }
    }

    public class AddExpClinicoCommandHandler
       : IRequestHandler<AddExpClinicoCommand, bool>
    {
        private readonly IGenericRepository<ExpedientesClinicos> _repository;

        public AddExpClinicoCommandHandler(
            IGenericRepository<ExpedientesClinicos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddExpClinicoCommand request,
            CancellationToken cancellationToken)
        {
            var expedientes = new ExpedientesClinicos
            {
                
                PacienteId = request.PacienteId,
                NumeroExpediente = request.NumeroExpediente,
                FechaApertura = request.FechaApertura,
                Estado = request.Estado,
                CreadoEn = request.CreadoEn,
                ActualizadoEn = request.ActualizadoEn

            };

            await _repository.AddAsync(expedientes);

            return true;
        }
    }
}
