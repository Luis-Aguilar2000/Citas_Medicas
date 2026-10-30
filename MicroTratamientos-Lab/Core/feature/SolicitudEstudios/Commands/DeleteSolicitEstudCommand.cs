using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SolicitudEstudios.Commands
{
    public class DeleteSolicitEstudCommand: IRequest<bool>
    {
        public int IdSolicitudEstudio { get; set; }
    }

    public class DeleteSolicitEstudCommandHandler
        : IRequestHandler<DeleteSolicitEstudCommand, bool>
    {
        private readonly IGenericRepository<SolicitudEstudio> _repository;

        public DeleteSolicitEstudCommandHandler(
            IGenericRepository<SolicitudEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteSolicitEstudCommand request,
            CancellationToken cancellationToken)
        {
            var solicitud = await _repository.GetByIdAsync(request.IdSolicitudEstudio);

            if (solicitud == null)
            {
                return false;
            }

            await _repository.DeleteAsync(solicitud);

            return true;
        }
    }
}
