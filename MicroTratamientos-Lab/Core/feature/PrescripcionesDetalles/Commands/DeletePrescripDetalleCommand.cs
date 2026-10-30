using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.PrescripcionesDetalles.Commands
{
    public class DeletePrescripDetalleCommand: IRequest<bool>
    {
        public int IdPrescripcionDetalle { get; set; }
    }

    public class DeletePrescripDetalleCommandHandler
       : IRequestHandler<DeletePrescripDetalleCommand, bool>
    {
        private readonly IGenericRepository<PrescripcionDetalle> _repository;

        public DeletePrescripDetalleCommandHandler(
            IGenericRepository<PrescripcionDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeletePrescripDetalleCommand request,
            CancellationToken cancellationToken)
        {
            var prescripcionDetalle = await _repository.GetByIdAsync(request.IdPrescripcionDetalle);

            if (prescripcionDetalle == null)
            {
                return false;
            }

            await _repository.DeleteAsync(prescripcionDetalle);

            return true;
        }
    }
}
