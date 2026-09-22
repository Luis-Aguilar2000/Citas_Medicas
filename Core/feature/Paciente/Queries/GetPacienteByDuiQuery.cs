using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Paciente.Queries
{
    public class GetPacienteByDuiQuery : IRequest<Pacientes?>
    {
        public string DUI { get; set; } = string.Empty;
    }

    public class GetPacienteByDuiQueryHandler
        : IRequestHandler<GetPacienteByDuiQuery, Pacientes?>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public GetPacienteByDuiQueryHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<Pacientes?> Handle(
            GetPacienteByDuiQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetOneByAsync(
                x => x.DUI == request.DUI,
                cancellationToken: cancellationToken
            );
        }
    }
}