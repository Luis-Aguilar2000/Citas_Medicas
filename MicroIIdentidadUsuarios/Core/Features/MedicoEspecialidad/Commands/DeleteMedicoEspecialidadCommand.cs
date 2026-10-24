using Generics.Interfaces;
using MediatR;
using MedicoEspecialidadModel = Domain.Models.MedicoEspecialidad;

namespace Core.feature.MedicoEspecialidad.Commands
{
    public class DeleteMedicoEspecialidadCommand : IRequest<bool>
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
    }

    public class DeleteMedicoEspecialidadCommandHandler
        : IRequestHandler<DeleteMedicoEspecialidadCommand, bool>
    {
        private readonly IGenericRepository<MedicoEspecialidadModel> _repository;

        public DeleteMedicoEspecialidadCommandHandler(
            IGenericRepository<MedicoEspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteMedicoEspecialidadCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetOneByAsync(
                x => x.IdMedico == request.IdMedico &&
                     x.IdEspecialidad == request.IdEspecialidad,
                cancellationToken: cancellationToken);

            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity);

            return true;
        }
    }
}