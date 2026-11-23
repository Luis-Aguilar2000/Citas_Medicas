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
    public class AddNotasMedCommand : IRequest<bool>
    {
        public int NotaId { get; set; }
        public int AtencionId { get; set; }
        public string? Subjetivo { get; set; }
        public string? Objetivo { get; set; }
        public string? Analisis { get; set; }
        public string? Plan { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class AddNotasMedCommandHandler
       : IRequestHandler<AddNotasMedCommand, bool>
    {
        private readonly IGenericRepository<NotasMedicas> _repository;

        public AddNotasMedCommandHandler(
            IGenericRepository<NotasMedicas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddNotasMedCommand request,
            CancellationToken cancellationToken)
        {
            var notas = new NotasMedicas
            {
                AtencionId = request.AtencionId,
                Subjetivo = request.Subjetivo,
                Objetivo = request.Objetivo,
                Analisis = request.Analisis,
                Plan = request.Plan,
                FechaCreacion = request.FechaCreacion


            };

            await _repository.AddAsync(notas);

            return true;
        }
    }
}
