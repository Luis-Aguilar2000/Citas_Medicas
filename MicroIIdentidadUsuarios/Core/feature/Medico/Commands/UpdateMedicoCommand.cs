using Generics.Interfaces;
using MediatR;
using MedicoModel = Domain.Models.Medico;

namespace Core.feature.Medico.Commands
{
    public class UpdateMedicoCommand : IRequest<bool>
    {
        public int IdMedico { get; set; }
        public int IdUsuario { get; set; }
        public string NumeroJVPM { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime? FechaSalida { get; set; }
    }

    public class UpdateMedicoCommandHandler
        : IRequestHandler<UpdateMedicoCommand, bool>
    {
        private readonly IGenericRepository<MedicoModel> _repository;

        public UpdateMedicoCommandHandler(
            IGenericRepository<MedicoModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateMedicoCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdMedico);

            if (entity == null)
                return false;

            entity.IdUsuario = request.IdUsuario;
            entity.NumeroJVPM = request.NumeroJVPM;
            entity.Estado = request.Estado;
            entity.FechaSalida = request.FechaSalida;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}