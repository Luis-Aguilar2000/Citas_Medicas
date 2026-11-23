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
    public class AddHabitosCommand : IRequest<bool>
    {
        public int HabitoId { get; set; }
        public int ExpedienteId { get; set; }
        public string TipoHabito { get; set; } = string.Empty;
        public string? Frecuencia { get; set; }
        public string? Descripcion { get; set; }
    }

    public class AddHabitosCommandHandler
       : IRequestHandler<AddHabitosCommand, bool>
    {
        private readonly IGenericRepository<Habitos> _repository;

        public AddHabitosCommandHandler(
            IGenericRepository<Habitos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddHabitosCommand request,
            CancellationToken cancellationToken)
        {
            var habitos = new Habitos
            {
                ExpedienteId = request.ExpedienteId,
                TipoHabito = request.TipoHabito,
                Frecuencia = request.Frecuencia,
                Descripcion = request.Descripcion,
                


            };

            await _repository.AddAsync(habitos);

            return true;
        }
    }
}
