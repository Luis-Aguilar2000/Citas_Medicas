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
    public class AddSintomasCommand : IRequest<bool>
    {
        public int SintomaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class AddSintomasCommandHandler
       : IRequestHandler<AddSintomasCommand, bool>
    {
        private readonly IGenericRepository<Sintomas> _repository;

        public AddSintomasCommandHandler(
            IGenericRepository<Sintomas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddSintomasCommand request,
            CancellationToken cancellationToken)
        {
            var sintomas = new Sintomas
            {
                SintomaId = request.SintomaId,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion


            };

            await _repository.AddAsync(sintomas);

            return true;
        }
    }
}
