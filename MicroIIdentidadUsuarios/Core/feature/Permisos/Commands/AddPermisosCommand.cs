using Generics.Interfaces;
using MediatR;
using PermisosModel = Domain.Models.Permisos;

namespace Core.feature.Permisos.Commands
{
    public class AddPermisosCommand : IRequest<bool>
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class AddPermisosCommandHandler
        : IRequestHandler<AddPermisosCommand, bool>
    {
        private readonly IGenericRepository<PermisosModel> _repository;

        public AddPermisosCommandHandler(
            IGenericRepository<PermisosModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddPermisosCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new PermisosModel
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Estado = request.Estado
            };

            await _repository.AddAsync(entity);

            return true;
        }
    }
}