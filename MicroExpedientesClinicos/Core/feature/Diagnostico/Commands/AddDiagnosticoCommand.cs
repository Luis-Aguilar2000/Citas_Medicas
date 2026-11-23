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
    public class AddDiagnosticoCommand : IRequest<bool>
    {
        public int DiagnosticoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class AddDiagnosticoCommandHandler
       : IRequestHandler<AddDiagnosticoCommand, bool>
    {
        private readonly IGenericRepository<Diagnosticos> _repository;

        public AddDiagnosticoCommandHandler(
            IGenericRepository<Diagnosticos> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddDiagnosticoCommand request,
            CancellationToken cancellationToken)
        {
            var diagnosticos = new Diagnosticos
            {
                
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,

            };

            await _repository.AddAsync(diagnosticos);

            return true;
        }
    }
}
