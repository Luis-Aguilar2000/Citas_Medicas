using Generics.Interfaces;
using MediatR;
using SesionUsuarioModel = Domain.Models.SesionUsuario;

namespace Core.feature.SesionUsuario.Queries
{
    public class GetSesionUsuarioByIdQuery : IRequest<SesionUsuarioModel?>
    {
        public int IdSesion { get; set; }
    }

    public class GetSesionUsuarioByIdQueryHandler
        : IRequestHandler<GetSesionUsuarioByIdQuery, SesionUsuarioModel?>
    {
        private readonly IGenericRepository<SesionUsuarioModel> _repository;

        public GetSesionUsuarioByIdQueryHandler(
            IGenericRepository<SesionUsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<SesionUsuarioModel?> Handle(
            GetSesionUsuarioByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdSesion);
        }
    }
}