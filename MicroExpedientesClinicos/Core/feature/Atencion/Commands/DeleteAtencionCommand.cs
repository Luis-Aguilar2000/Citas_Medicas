using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Atencion.Commands
{
    

    public class DeleteAtencionCommand : IRequest<bool>
    {
        public int AtencionId { get; set; }
    }

    public class DeleteAtencionCommandHandler
       : IRequestHandler<DeleteAtencionCommand, bool>
    {
        private readonly IGenericRepository<Atenciones> _repository;

        public DeleteAtencionCommandHandler(
            IGenericRepository<Atenciones> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteAtencionCommand request,
            CancellationToken cancellationToken)
        {
            var atencion = await _repository.GetByIdAsync(request.AtencionId);

            if (atencion == null)
            {
                return false;
            }

            await _repository.DeleteAsync(atencion);

            return true;
        }
    }
}
