
using Core.Services;
using MediatR;

namespace Core.feature.Citas.Queries
{
    public class GetMedicosDisponiblesQuery
        : IRequest<List<MedicoCatalogoResponse>>
    {
    }

    public class GetMedicosDisponiblesQueryHandler
        : IRequestHandler<
            GetMedicosDisponiblesQuery,
            List<MedicoCatalogoResponse>>
    {
        private readonly IIdentidadService _identidadService;

        public GetMedicosDisponiblesQueryHandler(
            IIdentidadService identidadService)
        {
            _identidadService = identidadService;
        }

        public async Task<List<MedicoCatalogoResponse>> Handle(
            GetMedicosDisponiblesQuery request,
            CancellationToken cancellationToken)
        {
            return await _identidadService.ObtenerMedicosAsync(
                cancellationToken);
        }
    }
}
