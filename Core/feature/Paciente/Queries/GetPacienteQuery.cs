using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using Generics.Filter;

namespace Core.feature.Paciente.Queries
{
    public class GetPacienteQuery
        : RequestParametersGets,
          IRequest<PagedResult<Pacientes>>
    {

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Filter { get; set; }

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
                ? Generics.Filter.Filter.FromStringExpression<Pacientes>(request.Filter) : null;
        
            

            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                filter: filter,
                cancellationToken: cancellationToken
                
            );
        }
    }
}