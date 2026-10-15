using Generics.Interfaces;
using MediatR;
using RolModel = Domain.Models.Rol;

namespace Core.feature.Rol.Commands
{
    public class DeleteRolCommand : IRequest<bool>
    {
        public int IdRol { get; set; }
    }

    public class DeleteRolCommandHandler
        : IRequestHandler<DeleteRolCommand, bool>
    {
        private readonly IGenericRepository<RolModel> _repository;

        public DeleteRolCommandHandler(
            IGenericRepository<RolModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteRolCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdRol);

            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}