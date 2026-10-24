using Generics.Interfaces;
using MediatR;
using MedicoEspecialidadModel = Domain.Models.MedicoEspecialidad;

namespace Core.feature.MedicoEspecialidad.Commands
{
    public class AddMedicoEspecialidadCommand : IRequest<bool>
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
    }

    public class AddMedicoEspecialidadCommandHandler
        : IRequestHandler<AddMedicoEspecialidadCommand, bool>
    {
        private readonly IGenericRepository<MedicoEspecialidadModel> _repository;

        public AddMedicoEspecialidadCommandHandler(
            IGenericRepository<MedicoEspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddMedicoEspecialidadCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new MedicoEspecialidadModel
            {
                IdMedico = request.IdMedico,
                IdEspecialidad = request.IdEspecialidad
            };

            await _repository.AddAsync(entity);

            return true;
        }
    }
}