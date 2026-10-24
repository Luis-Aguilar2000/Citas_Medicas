using Generics.Interfaces;
using MediatR;
using RolModel = Domain.Models.Rol;

namespace Core.feature.Rol.Queries
{
    public class GetRolByIdQuery : IRequest<RolModel?>
    {
        public int IdRol { get; set; }
    }

    public class GetRolByIdQueryHandler
        : IRequestHandler<GetRolByIdQuery, RolModel?>
    {
        private readonly IGenericRepository<RolModel> _repository;

        public GetRolByIdQueryHandler(
            IGenericRepository<RolModel> repository)
        {
            _repository = repository;
        }

        public async Task<RolModel?> Handle(
            GetRolByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdRol);
        }
    }
}