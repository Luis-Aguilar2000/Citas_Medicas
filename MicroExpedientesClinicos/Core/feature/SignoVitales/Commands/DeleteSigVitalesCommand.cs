using Core.feature.NotasMedica.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SignoVitales.Commands
{
    public class DeleteSigVitalesCommand : IRequest<bool>
    {
        public int SignosVitalesId { get; set; }
    }

    public class DeleteSigVitalesCommandHandler
       : IRequestHandler<DeleteSigVitalesCommand, bool>
    {
        private readonly IGenericRepository<SignosVitales> _repository;

        public DeleteSigVitalesCommandHandler(
            IGenericRepository<SignosVitales> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteSigVitalesCommand request,
            CancellationToken cancellationToken)
        {
            var signos = await _repository.GetByIdAsync(request.SignosVitalesId);

            if (signos == null)
            {
                return false;
            }

            await _repository.DeleteAsync(signos);

            return true;
        }
    }
}
