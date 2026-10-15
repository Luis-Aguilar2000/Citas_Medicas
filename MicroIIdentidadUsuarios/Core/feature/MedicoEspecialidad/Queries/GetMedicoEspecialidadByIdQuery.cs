using Generics.Interfaces;
using MediatR;
using MedicoEspecialidadModel = Domain.Models.MedicoEspecialidad;

namespace Core.feature.MedicoEspecialidad.Queries
{
    public class GetMedicoEspecialidadByIdQuery
        : IRequest<MedicoEspecialidadModel?>
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
    }

    public class GetMedicoEspecialidadByIdQueryHandler
        : IRequestHandler<GetMedicoEspecialidadByIdQuery,
            MedicoEspecialidadModel?>
    {
        private readonly IGenericRepository<MedicoEspecialidadModel> _repository;

        public GetMedicoEspecialidadByIdQueryHandler(
            IGenericRepository<MedicoEspecialidadModel> repository)
        {
            _repository = repository;
        }

        public async Task<MedicoEspecialidadModel?> Handle(
            GetMedicoEspecialidadByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetOneByAsync(
                x => x.IdMedico == request.IdMedico &&
                     x.IdEspecialidad == request.IdEspecialidad,
                cancellationToken: cancellationToken);
        }
    }
}