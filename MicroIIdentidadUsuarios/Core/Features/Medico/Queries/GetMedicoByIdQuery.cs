using Generics.Interfaces;
using MediatR;
using MedicoModel = Domain.Models.Medico;

namespace Core.feature.Medico.Queries
{
    public class GetMedicoByIdQuery : IRequest<MedicoModel?>
    {
        public int IdMedico { get; set; }
    }

    public class GetMedicoByIdQueryHandler
        : IRequestHandler<GetMedicoByIdQuery, MedicoModel?>
    {
        private readonly IGenericRepository<MedicoModel> _repository;

        public GetMedicoByIdQueryHandler(
            IGenericRepository<MedicoModel> repository)
        {
            _repository = repository;
        }

        public async Task<MedicoModel?> Handle(
            GetMedicoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdMedico);
        }
    }
}