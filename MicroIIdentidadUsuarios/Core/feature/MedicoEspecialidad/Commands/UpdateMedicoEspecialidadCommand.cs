using Generics.Interfaces;
using MediatR;
using MedicoEspecialidadModel = Domain.Models.MedicoEspecialidad;

namespace Core.feature.MedicoEspecialidad.Commands
{
    public class UpdateMedicoEspecialidadCommand : IRequest<bool>
    {
        public int IdMedicoActual { get; set; }
        public int IdEspecialidadActual { get; set; }

        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
    }

    public class UpdateMedicoEspecialidadCommandHandler
        : IRequestHandler<UpdateMedicoEspecialidadCommand, bool>
    {
        private readonly IGenericRepository<MedicoEspecialidadModel> _repository;

        public UpdateMedicoEspecialidadCommandHandler(
            IGenericRepository<MedicoEspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateMedicoEspecialidadCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetOneByAsync(
                x => x.IdMedico == request.IdMedicoActual &&
                     x.IdEspecialidad == request.IdEspecialidadActual,
                asNoTracking: false,
                cancellationToken: cancellationToken);

            if (entity == null)
                return false;

            entity.IdMedico = request.IdMedico;
            entity.IdEspecialidad = request.IdEspecialidad;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}