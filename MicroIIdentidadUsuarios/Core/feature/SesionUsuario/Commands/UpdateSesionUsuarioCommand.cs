using Generics.Interfaces;
using MediatR;
using SesionUsuarioModel = Domain.Models.SesionUsuario;

namespace Core.feature.SesionUsuario.Commands
{
    public class UpdateSesionUsuarioCommand : IRequest<bool>
    {
        public int IdSesion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Estado { get; set; }
    }

    public class UpdateSesionUsuarioCommandHandler
        : IRequestHandler<UpdateSesionUsuarioCommand, bool>
    {
        private readonly IGenericRepository<SesionUsuarioModel> _repository;

        public UpdateSesionUsuarioCommandHandler(
            IGenericRepository<SesionUsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateSesionUsuarioCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdSesion);

            if (entity == null)
                return false;

            entity.IdUsuario = request.IdUsuario;
            entity.FechaInicio = request.FechaInicio;
            entity.FechaExpiracion = request.FechaExpiracion;
            entity.Estado = request.Estado;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}