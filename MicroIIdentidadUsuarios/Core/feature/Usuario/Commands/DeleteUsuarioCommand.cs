using Generics.Interfaces;
using MediatR;
using UsuarioModel = Domain.Models.Usuario;

namespace Core.feature.Usuario.Commands
{
    public class DeleteUsuarioCommand : IRequest<bool>
    {
        public int IdUsuario { get; set; }
    }

    public class DeleteUsuarioCommandHandler
        : IRequestHandler<DeleteUsuarioCommand, bool>
    {
        private readonly IGenericRepository<UsuarioModel> _repository;

        public DeleteUsuarioCommandHandler(
            IGenericRepository<UsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteUsuarioCommand request,
            CancellationToken cancellationToken)
        {
            var usuario =
                await _repository.GetByIdAsync(request.IdUsuario);

            if (usuario == null)
                return false;

            await _repository.DeleteAsync(usuario);

            return true;
        }
    }
}