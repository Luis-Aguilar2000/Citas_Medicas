using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Habitoss.Queries
{
    public class GetHabitosQuery
       : RequestParametersGets,
         IRequest<PagedResult<Habitos>>
    {
    }

    public class GetHabitosQueryHandler
       : IRequestHandler<GetHabitosQuery, PagedResult<Habitos>>
    {
        private readonly IGenericRepository<Habitos> _repository;

        public GetHabitosQueryHandler(
            IGenericRepository<Habitos> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Habitos>> Handle(
            GetHabitosQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrEmpty(request.Filter)
                ? Filter.FromStringExpression<Habitos>(request.Filter)
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
