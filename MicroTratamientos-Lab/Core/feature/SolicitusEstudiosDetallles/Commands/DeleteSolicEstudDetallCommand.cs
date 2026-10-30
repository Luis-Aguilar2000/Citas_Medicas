using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SolicitusEstudiosDetallles.Commands
{
    public class DeleteSolicEstudDetallCommand: IRequest<bool>
    {
        public int IdSolicitudEstudioDetalle { get; set; }
    }

    public class DeleteSolicEstudDetallCommandHandler
        : IRequestHandler<DeleteSolicEstudDetallCommand, bool>
    {
        private readonly IGenericRepository<SolicitudEstudioDetalle> _repository;

        public DeleteSolicEstudDetallCommandHandler(
            IGenericRepository<SolicitudEstudioDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteSolicEstudDetallCommand request,
            CancellationToken cancellationToken)
        {
            var solicitudEstudioDetalle = await _repository.GetByIdAsync(request.IdSolicitudEstudioDetalle);

            if (solicitudEstudioDetalle == null)
            {
                return false;
            }

            await _repository.DeleteAsync(solicitudEstudioDetalle);

            return true;
        }
    }
}
