using Generics.Interfaces;
using MediatR;
using SesionUsuarioModel = Domain.Models.SesionUsuario;

namespace Core.feature.SesionUsuario.Commands
{
    public class AddSesionUsuarioCommand : IRequest<bool>
    {
        public int IdUsuario { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Estado { get; set; }
    }

    public class AddSesionUsuarioCommandHandler
        : IRequestHandler<AddSesionUsuarioCommand, bool>
    {
        private readonly IGenericRepository<SesionUsuarioModel> _repository;

        public AddSesionUsuarioCommandHandler(
            IGenericRepository<SesionUsuarioModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddSesionUsuarioCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new SesionUsuarioModel
            {
                IdUsuario = request.IdUsuario,
                FechaInicio = DateTime.Now,
                FechaExpiracion = request.FechaExpiracion,
                Estado = request.Estado
            };

            await _repository.AddAsync(entity);

            return true;
        }
    }
}