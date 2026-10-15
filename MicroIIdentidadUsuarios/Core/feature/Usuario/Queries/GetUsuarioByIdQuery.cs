using Generics.Interfaces;
using MediatR;
using UsuarioModel = Domain.Models.Usuario;

namespace Core.feature.Usuario.Queries
{
    public class GetUsuarioByIdQuery : IRequest<UsuarioModel?>
    {
        public int IdUsuario { get; set; }
    }

    public class GetUsuarioByIdQueryHandler
        : IRequestHandler<GetUsuarioByIdQuery, UsuarioModel?>
    {
        private readonly IGenericRepository<UsuarioModel> _repository;

        public GetUsuarioByIdQueryHandler(
            IGenericRepository<UsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<UsuarioModel?> Handle(
            GetUsuarioByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdUsuario);
        }
    }
}