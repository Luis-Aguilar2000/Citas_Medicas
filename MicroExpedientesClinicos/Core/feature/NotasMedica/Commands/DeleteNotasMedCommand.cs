using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.NotasMedica.Commands
{

    public class DeleteNotasMedCommand : IRequest<bool>
    {
        public int NotaId { get; set; }
    }

    public class DeleteNotasMedCommandHandler
       : IRequestHandler<DeleteNotasMedCommand, bool>
    {
        private readonly IGenericRepository<NotasMedicas> _repository;

        public DeleteNotasMedCommandHandler(
            IGenericRepository<NotasMedicas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteNotasMedCommand request,
            CancellationToken cancellationToken)
        {
            var notas = await _repository.GetByIdAsync(request.NotaId);

            if (notas == null)
            {
                return false;
            }

            await _repository.DeleteAsync(notas);

            return true;
        }
    }
}
