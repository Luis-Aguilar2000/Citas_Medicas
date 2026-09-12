using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Paciente.Commands
{
    public class DeletePacienteCommand : IRequest<bool>
    {
        public int IdPaciente { get; set; }
    }

    public class DeletePacienteCommandHandler
        : IRequestHandler<DeletePacienteCommand, bool>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public DeletePacienteCommandHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeletePacienteCommand request,
            CancellationToken cancellationToken)
        {
            // Buscar el paciente
            var paciente = await _repository.GetByIdAsync(
                request.IdPaciente
            );

            // Si no existe
            if (paciente == null)
            {
                return false;
            }

            // Eliminar paciente
            await _repository.DeleteAsync(paciente);

            return true;
        }
    }
}