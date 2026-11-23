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

    public class DeleteHabitosCommand : IRequest<bool>
    {
        public int HabitoId { get; set; }
    }

    public class DeleteHabitosCommandHandler
       : IRequestHandler<DeleteHabitosCommand, bool>
    {
        private readonly IGenericRepository<Habitos> _repository;

        public DeleteHabitosCommandHandler(
            IGenericRepository<Habitos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteHabitosCommand request,
            CancellationToken cancellationToken)
        {
            var habitos = await _repository.GetByIdAsync(request.HabitoId);

            if (habitos == null)
            {
                return false;
            }

            await _repository.DeleteAsync(habitos);

            return true;
        }
    }
}
