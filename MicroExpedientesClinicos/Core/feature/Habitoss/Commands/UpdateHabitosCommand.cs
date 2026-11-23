using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Habitoss.Commands
{
    public class UpdateHabitosCommand : IRequest<bool>
    {
        public int HabitoId { get; set; }
        public int ExpedienteId { get; set; }
        public string TipoHabito { get; set; } = string.Empty;
        public string? Frecuencia { get; set; }
        public string? Descripcion { get; set; }
    }

    public class UpdateHabitosCommandHandler
       : IRequestHandler<UpdateHabitosCommand, bool>
    {
        private readonly IGenericRepository<Habitos> _repository;

        public UpdateHabitosCommandHandler(
            IGenericRepository<Habitos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateHabitosCommand request,
            CancellationToken cancellationToken)
        {
            var habitos = await _repository.GetByIdAsync(request.HabitoId);

            if (habitos == null)
            {
                return false;
            }

            habitos.ExpedienteId = request.ExpedienteId;
            habitos.TipoHabito = request.TipoHabito;
            habitos.Frecuencia = request.Frecuencia;
            habitos.Descripcion = request.Descripcion;

            await _repository.UpdateAsync(habitos);

            return true;
        }
    }
}
