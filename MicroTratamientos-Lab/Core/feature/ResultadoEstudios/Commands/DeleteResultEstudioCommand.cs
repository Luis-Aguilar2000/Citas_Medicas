using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ResultadoEstudios.Commands
{
    public class DeleteResultEstudioCommand: IRequest<bool>
    {
        public int IdResultadoEstudio { get; set; }
    }

    public class DeleteResultEstudioCommandHandler
       : IRequestHandler<DeleteResultEstudioCommand, bool>
    {
        private readonly IGenericRepository<ResultadoEstudio> _repository;

        public DeleteResultEstudioCommandHandler(
            IGenericRepository<ResultadoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteResultEstudioCommand request,
            CancellationToken cancellationToken)
        {
            var resultado = await _repository.GetByIdAsync(request.IdResultadoEstudio);

            if (resultado == null)
            {
                return false;
            }

            await _repository.DeleteAsync(resultado);

            return true;
        }
    }
}
