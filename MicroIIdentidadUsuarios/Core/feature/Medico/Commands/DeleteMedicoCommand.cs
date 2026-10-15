using Generics.Interfaces;
using MediatR;
using MedicoModel = Domain.Models.Medico;

namespace Core.feature.Medico.Commands
{
    public class DeleteMedicoCommand : IRequest<bool>
    {
        public int IdMedico { get; set; }
    }

    public class DeleteMedicoCommandHandler
        : IRequestHandler<DeleteMedicoCommand, bool>
    {
        private readonly IGenericRepository<MedicoModel> _repository;

        public DeleteMedicoCommandHandler(
            IGenericRepository<MedicoModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteMedicoCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdMedico);

            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}