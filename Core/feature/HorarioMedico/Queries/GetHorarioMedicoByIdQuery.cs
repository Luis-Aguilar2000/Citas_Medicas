using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.HorarioMedico.Queries
{
    public class GetHorarioMedicoByIdQuery
        : IRequest<HorariosMedico?>
    {
        public int IdHorario { get; set; }
    }

    public class GetHorarioMedicoByIdQueryHandler
        : IRequestHandler<GetHorarioMedicoByIdQuery, HorariosMedico?>
    {
        private readonly IGenericRepository<HorariosMedico> _repository;

        public GetHorarioMedicoByIdQueryHandler(
            IGenericRepository<HorariosMedico> repository)
        {
            _repository = repository;
        }

        public async Task<HorariosMedico?> Handle(
            GetHorarioMedicoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdHorario);
        }
    }
}
