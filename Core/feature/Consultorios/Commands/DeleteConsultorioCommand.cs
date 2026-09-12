using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Consultorios.Commands
{
    public class DeleteConsultorioCommand : IRequest<bool>
    {
        public int IdConsultorio { get; set; }
    }


    public class DeleteConsultorioCommandHandler
        : IRequestHandler<DeleteConsultorioCommand, bool>
    {
        private readonly IGenericRepository<Consultorio> _repository;


        public DeleteConsultorioCommandHandler(
            IGenericRepository<Consultorio> repository)
        {
            _repository = repository;
        }


        public async Task<bool> Handle(
            DeleteConsultorioCommand request,
            CancellationToken cancellationToken)
        {
            var consultorio =
                await _repository.GetByIdAsync(
                    request.IdConsultorio
                );


            if (consultorio == null)
            {
                return false;
            }


            await _repository.DeleteAsync(
                consultorio
            );


            return true;
        }
    }
}