using Generics.Interfaces;
using MediatR;
using PermisosModel = Domain.Models.Permisos;

namespace Core.feature.Permisos.Commands
{
    public class DeletePermisosCommand : IRequest<bool>
    {
        public int IdPermiso { get; set; }
    }

    public class DeletePermisosCommandHandler
        : IRequestHandler<DeletePermisosCommand, bool>
    {
        private readonly IGenericRepository<PermisosModel> _repository;

        public DeletePermisosCommandHandler(
            IGenericRepository<PermisosModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeletePermisosCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdPermiso);

            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity);

            return true;
        }
    }
}