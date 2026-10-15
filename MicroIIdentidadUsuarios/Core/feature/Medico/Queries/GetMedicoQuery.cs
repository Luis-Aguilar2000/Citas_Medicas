using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using MedicoModel = Domain.Models.Medico;

namespace Core.feature.Medico.Queries
{
    public class GetMedicoQuery
        : RequestParametersGets,
          IRequest<PagedResult<MedicoModel>>
    {
    }

    public class GetMedicoQueryHandler
        : IRequestHandler<GetMedicoQuery, PagedResult<MedicoModel>>
    {
        private readonly IGenericRepository<MedicoModel> _repository;

        public GetMedicoQueryHandler(
            IGenericRepository<MedicoModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<MedicoModel>> Handle(
            GetMedicoQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<MedicoModel>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                cancellationToken: cancellationToken);
        }
    }
}