using Core.Facturacion.Exceptions;
using Core.Facturacion.Services;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Facturacion.Features.Facturas.Commands
{
    public class AddFacturaCommand : IRequest<Factura>
    {
        public int IdPaciente { get; set; }

        public DateTime FechaFactura { get; set; }

        public string Estado { get; set; } = string.Empty;
    }

    public class AddFacturaCommandHandler
        : IRequestHandler<AddFacturaCommand, Factura>
    {
        private readonly IGenericRepository<Factura> _repository;
        private readonly IPacienteService _pacienteService;

        public AddFacturaCommandHandler(
            IGenericRepository<Factura> repository,
            IPacienteService pacienteService)
        {
            _repository = repository;
            _pacienteService = pacienteService;
        }

        public async Task<Factura> Handle(
            AddFacturaCommand request,
            CancellationToken cancellationToken)
        {
            var existe = await _pacienteService.ExisteAsync(
                request.IdPaciente,
                cancellationToken);

            if (!existe)
            {
                throw new DomainException(
                    Errores.PACIENTE_NO_EXISTE,
                    $"El paciente {request.IdPaciente} no existe.");
            }

            var factura = new Factura
            {
                IdPaciente = request.IdPaciente,
                FechaFactura = request.FechaFactura,
                Estado = request.Estado
            };

            return await _repository.AddAsync(factura);
        }
    }
}