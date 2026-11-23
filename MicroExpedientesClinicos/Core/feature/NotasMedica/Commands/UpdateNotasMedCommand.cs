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
    public class UpdateNotasMedCommand : IRequest<bool>
    {
        public int NotaId { get; set; }
        public int AtencionId { get; set; }
        public string? Subjetivo { get; set; }
        public string? Objetivo { get; set; }
        public string? Analisis { get; set; }
        public string? Plan { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class UpdateNotasMedCommandHandler
       : IRequestHandler<UpdateNotasMedCommand, bool>
    {
        private readonly IGenericRepository<NotasMedicas> _repository;

        public UpdateNotasMedCommandHandler(
            IGenericRepository<NotasMedicas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateNotasMedCommand request,
            CancellationToken cancellationToken)
        {
            var notas = await _repository.GetByIdAsync(request.NotaId);

            if (notas == null)
            {
                return false;
            }

            notas.AtencionId = request.AtencionId;
            notas.Subjetivo = request.Subjetivo;
            notas.Objetivo = request.Objetivo;
            notas.Analisis = request.Analisis;
            notas.Plan = request.Plan;
            notas.FechaCreacion = request.FechaCreacion;

            await _repository.UpdateAsync(notas);

            return true;
        }
    }
}
