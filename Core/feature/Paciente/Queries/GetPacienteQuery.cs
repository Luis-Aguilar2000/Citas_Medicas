using Generics.Interfaces;
using Generics.Models;
using Domain.Models;
using MediatR;

namespace Core.feature.Paciente.Queries
{
    public class GetPacienteQuery : IRequest<PagedResult<Pacientes>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class GetPacienteQueryHandler
        : IRequestHandler<GetPacienteQuery, PagedResult<Pacientes>>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public GetPacienteQueryHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Pacientes>> Handle(
            GetPacienteQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken
            );
        }
    }
}