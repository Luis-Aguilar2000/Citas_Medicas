using Generics.Interfaces;
using MediatR;
using EspecialidadModel = Domain.Models.Especialidad;

namespace Core.feature.Especialidad.Queries
{
    public class GetEspecialidadByIdQuery : IRequest<EspecialidadModel?>
    {
        public int IdEspecialidad { get; set; }
    }

    public class GetEspecialidadByIdQueryHandler
        : IRequestHandler<GetEspecialidadByIdQuery, EspecialidadModel?>
    {
        private readonly IGenericRepository<EspecialidadModel> _repository;

        public GetEspecialidadByIdQueryHandler(
            IGenericRepository<EspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<EspecialidadModel?> Handle(
            GetEspecialidadByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdEspecialidad);
        }
    }
}