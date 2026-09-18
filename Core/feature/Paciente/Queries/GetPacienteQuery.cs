using Domain.Models;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using Generics.Helpers;

namespace Core.feature.Paciente.Queries
{
    public class GetPacienteQuery
        : RequestParametersGets,
          IRequest<PagedResult<Pacientes>>
    {
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
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Pacientes>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                filter: filter,
                cancellationToken: cancellationToken
            );
        }
    }
}