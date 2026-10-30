using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Prescripciones.Commands
{
    public class DeletePrescripCommand: IRequest<bool>
    {
        public int IdPrescripcion { get; set; }
    }

    public class DeletePrescripCommandHandler
       : IRequestHandler<DeletePrescripCommand, bool>
    {
        private readonly IGenericRepository<Prescripcion> _repository;

        public DeletePrescripCommandHandler(
            IGenericRepository<Prescripcion> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeletePrescripCommand request,
            CancellationToken cancellationToken)
        {
            var prescripcion = await _repository.GetByIdAsync(request.IdPrescripcion);

            if (prescripcion == null)
            {
                return false;
            }

            await _repository.DeleteAsync(prescripcion);

            return true;
        }
    }
}
