using Generics.Interfaces;
using MediatR;
using SesionUsuarioModel = Domain.Models.SesionUsuario;

namespace Core.feature.SesionUsuario.Commands
{
    public class DeleteSesionUsuarioCommand : IRequest<bool>
    {
        public int IdSesion { get; set; }
    }

    public class DeleteSesionUsuarioCommandHandler
        : IRequestHandler<DeleteSesionUsuarioCommand, bool>
    {
        private readonly IGenericRepository<SesionUsuarioModel> _repository;

        public DeleteSesionUsuarioCommandHandler(
            IGenericRepository<SesionUsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteSesionUsuarioCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdSesion);

            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity);

            return true;
        }
    }
}