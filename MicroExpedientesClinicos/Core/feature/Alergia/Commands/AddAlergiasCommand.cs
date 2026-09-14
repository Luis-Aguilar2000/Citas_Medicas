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
    public class AddAlergiasCommand: IRequest<bool>
    {
        public int AlergiaId { get; set; }
        public int ExpedienteId { get; set; }
        public string Sustancia { get; set; } = string.Empty;
        public string? Severidad { get; set; }
        public string? Reaccion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class AddAlergiasCommandHandler
       : IRequestHandler<AddAlergiasCommand, bool>
    {
        private readonly IGenericRepository<Alergias> _repository;

        public AddAlergiasCommandHandler(
            IGenericRepository<Alergias> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddAlergiasCommand request,
            CancellationToken cancellationToken)
        {
            var alergias = new Alergias
            {
               ExpedienteId = request.ExpedienteId,
               Sustancia = request.Sustancia,
               Severidad = request.Severidad,
               Reaccion = request.Reaccion,
               Estado = request.Estado,
               
            };

            await _repository.AddAsync(alergias);

            return true;
        }
    }
}
