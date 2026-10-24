using Generics.Interfaces;
using MediatR;
using EspecialidadModel = Domain.Models.Especialidad;

namespace Core.feature.Especialidad.Commands
{
    public class DeleteEspecialidadCommand : IRequest<bool>
    {
        public int IdEspecialidad { get; set; }
    }

    public class DeleteEspecialidadCommandHandler
        : IRequestHandler<DeleteEspecialidadCommand, bool>
    {
        private readonly IGenericRepository<EspecialidadModel> _repository;

        public DeleteEspecialidadCommandHandler(
            IGenericRepository<EspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteEspecialidadCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdEspecialidad);

            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}