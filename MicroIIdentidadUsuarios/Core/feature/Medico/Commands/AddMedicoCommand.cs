using Generics.Interfaces;
using MediatR;
using MedicoModel = Domain.Models.Medico;

namespace Core.feature.Medico.Commands
{
    public class AddMedicoCommand : IRequest<bool>
    {
        public int IdUsuario { get; set; }
        public string NumeroJVPM { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }

    public class AddMedicoCommandHandler
        : IRequestHandler<AddMedicoCommand, bool>
    {
        private readonly IGenericRepository<MedicoModel> _repository;

        public AddMedicoCommandHandler(
            IGenericRepository<MedicoModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddMedicoCommand request,
            CancellationToken cancellationToken)
        {
            var medico = new MedicoModel
            {
                IdUsuario = request.IdUsuario,
                NumeroJVPM = request.NumeroJVPM,
                Estado = request.Estado,
                FechaRegistro = DateTime.Now,
                FechaSalida = null
            };

            await _repository.AddAsync(medico);
            return true;
        }
    }
}