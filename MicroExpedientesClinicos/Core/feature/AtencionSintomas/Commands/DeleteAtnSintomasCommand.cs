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
    public class DeleteAtnSintomasCommand : IRequest<bool>
    {
        public int AtencionSintomaId { get; set; }
    }

    public class DeleteAtnSintomasCommandHandler
       : IRequestHandler<DeleteAtnSintomasCommand, bool>
    {
        private readonly IGenericRepository<AtencionSintoma> _repository;

        public DeleteAtnSintomasCommandHandler(
            IGenericRepository<AtencionSintoma> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteAtnSintomasCommand request,
            CancellationToken cancellationToken)
        {
            var atnSin = await _repository.GetByIdAsync(request.AtencionSintomaId);

            if (atnSin == null)
            {
                return false;
            }

            await _repository.DeleteAsync(atnSin);

            return true;
        }
    }
}
