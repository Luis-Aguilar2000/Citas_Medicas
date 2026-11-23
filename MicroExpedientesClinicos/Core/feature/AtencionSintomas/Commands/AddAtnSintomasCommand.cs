using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.AtencionSintomas.Commands
{
    public class AddAtnSintomasCommand : IRequest<bool>
    {
        public int AtencionSintomaId { get; set; }
        public int AtencionId { get; set; }
        public int SintomaId { get; set; }
        public string? Intensidad { get; set; }
        public string? Duracion { get; set; }
    }

    public class AddAtnSintomasCommandHandler
       : IRequestHandler<AddAtnSintomasCommand, bool>
    {
        private readonly IGenericRepository<AtencionSintoma> _repository;

        public AddAtnSintomasCommandHandler(
            IGenericRepository<AtencionSintoma> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddAtnSintomasCommand request,
            CancellationToken cancellationToken)
        {
            var atnSin = new AtencionSintoma
            {
                AtencionId = request.AtencionId,
                SintomaId = request.SintomaId,
                Intensidad = request.Intensidad,
                Duracion = request.Duracion


            };

            await _repository.AddAsync(atnSin);

            return true;
        }
    }
}
