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
    public class DeleteSintomasCommand : IRequest<bool>
    {
        public int SintomaId { get; set; }
        
    }

    public class DeleteSintomasCommandHandler
       : IRequestHandler<DeleteSintomasCommand, bool>
    {
        private readonly IGenericRepository<Sintomas> _repository;

        public DeleteSintomasCommandHandler(
            IGenericRepository<Sintomas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteSintomasCommand request,
            CancellationToken cancellationToken)
        {
            var sintomas= await _repository.GetByIdAsync(request.SintomaId);

            if (sintomas == null)
            {
                return false;
            }

            await _repository.DeleteAsync(sintomas);

            return true;
        }
    }
}
