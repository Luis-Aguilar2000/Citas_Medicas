using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Diagnostico.Commands
{

    public class UpdateDiagnosticoCommand : IRequest<bool>
    {
        public int DiagnosticoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class UpdateDiagnosticoCommandHandler
       : IRequestHandler<UpdateDiagnosticoCommand, bool>
    {
        private readonly IGenericRepository<Diagnosticos> _repository;

        public UpdateDiagnosticoCommandHandler(
            IGenericRepository<Diagnosticos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateDiagnosticoCommand request,
            CancellationToken cancellationToken)
        {
            var diagnosticos = await _repository.GetByIdAsync(request.DiagnosticoId);

            if (diagnosticos == null)
            {
                return false;
            }

            diagnosticos.Codigo = request.Codigo;
            diagnosticos.Nombre = request.Nombre;
            diagnosticos.Descripcion = request.Descripcion;


            await _repository.UpdateAsync(diagnosticos);

            return true;
        }
    }
}
