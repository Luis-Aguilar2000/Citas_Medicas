using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Sintomass.Commands
{

    public class UpdateSintomasCommand : IRequest<bool>
    {
        public int SintomaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class UpdateSintomasCommandHandler
       : IRequestHandler<UpdateSintomasCommand, bool>
    {
        private readonly IGenericRepository<Sintomas> _repository;

        public UpdateSintomasCommandHandler(
            IGenericRepository<Sintomas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateSintomasCommand request,
            CancellationToken cancellationToken)
        {
            var sintomas = await _repository.GetByIdAsync(request.SintomaId);

            if (sintomas == null)
            {
                return false;
            }

            sintomas.Nombre = request.Nombre;
            sintomas.Descripcion = request.Descripcion;
           


            await _repository.UpdateAsync(sintomas);

            return true;
        }
    }
}
