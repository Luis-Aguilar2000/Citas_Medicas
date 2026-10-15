using Generics.Interfaces;
using MediatR;
using EspecialidadModel = Domain.Models.Especialidad;

namespace Core.feature.Especialidad.Commands
{
    public class UpdateEspecialidadCommand : IRequest<bool>
    {
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class UpdateEspecialidadCommandHandler
        : IRequestHandler<UpdateEspecialidadCommand, bool>
    {
        private readonly IGenericRepository<EspecialidadModel> _repository;

        public UpdateEspecialidadCommandHandler(
            IGenericRepository<EspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateEspecialidadCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdEspecialidad);

            if (entity == null)
                return false;

            entity.Nombre = request.Nombre;
            entity.Descripcion = request.Descripcion;
            entity.Estado = request.Estado;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}