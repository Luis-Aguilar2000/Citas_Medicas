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


    public class UpdateAtnSintomasCommand : IRequest<bool>
    {
        public int AtencionSintomaId { get; set; }
        public int AtencionId { get; set; }
        public int SintomaId { get; set; }
        public string? Intensidad { get; set; }
        public string? Duracion { get; set; }
    }

    public class UpdateAtnSintomasCommandHandler
       : IRequestHandler<UpdateAtnSintomasCommand, bool>
    {
        private readonly IGenericRepository<AtencionSintoma> _repository;

        public UpdateAtnSintomasCommandHandler(
            IGenericRepository<AtencionSintoma> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateAtnSintomasCommand request,
            CancellationToken cancellationToken)
        {
            var atnSin = await _repository.GetByIdAsync(request.AtencionSintomaId);

            if (atnSin == null)
            {
                return false;
            }

            atnSin.AtencionId = request.AtencionId;
            atnSin.SintomaId = request.SintomaId;
            atnSin.Intensidad = request.Intensidad;
            atnSin.Intensidad = request.Intensidad;


            await _repository.UpdateAsync(atnSin);

            return true;

        }

    }
}
