using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Alergia.Commands
{
    public class UpdateAlergiasCommand: IRequest<bool>
    {
        public int AlergiaId { get; set; }
        public int ExpedienteId { get; set; }
        public string Sustancia { get; set; } = string.Empty;
        public string? Severidad { get; set; }
        public string? Reaccion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class UpdateAlergiasCommandHandler
        : IRequestHandler<UpdateAlergiasCommand, bool>
    {
        private readonly IGenericRepository<Alergias> _repository;

        public UpdateAlergiasCommandHandler(
            IGenericRepository<Alergias> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateAlergiasCommand request,
            CancellationToken cancellationToken)
        {
            var alergias = await _repository.GetByIdAsync(request.AlergiaId);

            if (alergias == null)
            {
                return false;
            }

            alergias.ExpedienteId = request.ExpedienteId;
            alergias.Sustancia = request.Sustancia;
            alergias.Severidad = request.Severidad;
            alergias.Reaccion = request.Reaccion;
            alergias.Estado = request.Estado;

            await _repository.UpdateAsync(alergias);

            return true;
        }
    }
}
