using Generics.Interfaces;
using MediatR;
using RolModel = Domain.Models.Rol;

namespace Core.feature.Rol.Commands
{
    public class AddRolCommand : IRequest<bool>
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class AddRolCommandHandler
        : IRequestHandler<AddRolCommand, bool>
    {
        private readonly IGenericRepository<RolModel> _repository;

        public AddRolCommandHandler(
            IGenericRepository<RolModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddRolCommand request,
            CancellationToken cancellationToken)
        {
            var rol = new RolModel
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Estado = request.Estado,
                FechaRegistro = DateTime.Now
            };

            await _repository.AddAsync(rol);

            return true;
        }
    }
}