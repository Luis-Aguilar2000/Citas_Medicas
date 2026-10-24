using Generics.Interfaces;
using MediatR;
using EspecialidadModel = Domain.Models.Especialidad;

namespace Core.feature.Especialidad.Commands
{
    public class AddEspecialidadCommand : IRequest<bool>
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class AddEspecialidadCommandHandler
        : IRequestHandler<AddEspecialidadCommand, bool>
    {
        private readonly IGenericRepository<EspecialidadModel> _repository;

        public AddEspecialidadCommandHandler(
            IGenericRepository<EspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddEspecialidadCommand request,
            CancellationToken cancellationToken)
        {
            var especialidad = new EspecialidadModel
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Estado = request.Estado,
                FechaRegistro = DateTime.Now
            };

            await _repository.AddAsync(especialidad);
            return true;
        }
    }
}