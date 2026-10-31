using Core.Facturacion.Exceptions;
using Core.Facturacion.Services;
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.FacturaDetalles.Commands
{
    public class AddFacturaDetalleCommand : IRequest<FacturaDetalle>
    {
        public int IdFactura { get; set; }

        public string TipoItem { get; set; } = string.Empty;

        public int IdItem { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }

    public class AddFacturaDetalleCommandHandler
        : IRequestHandler<AddFacturaDetalleCommand, FacturaDetalle>
    {
        private readonly IGenericRepository<FacturaDetalle> _repository;
        private readonly IGenericRepository<Factura> _facturaRepository;
        private readonly ITratamientoLabService _tratamientoLabService;

        public AddFacturaDetalleCommandHandler(
            IGenericRepository<FacturaDetalle> repository,
            IGenericRepository<Factura> facturaRepository,
            ITratamientoLabService tratamientoLabService)
        {
            _repository = repository;
            _facturaRepository = facturaRepository;
            _tratamientoLabService = tratamientoLabService;
        }

        public async Task<FacturaDetalle> Handle(
            AddFacturaDetalleCommand request,
            CancellationToken cancellationToken)
        {
            if (request.IdFactura <= 0)
            {
                throw new DomainException(
                    Errores.FACTURA_NO_EXISTE,
                    "La factura indicada no existe."
                );
            }

            var factura = await _facturaRepository.GetByIdAsync(
                request.IdFactura,
                cancellationToken
            );

            if (factura == null)
            {
                throw new DomainException(
                    Errores.FACTURA_NO_EXISTE,
                    "La factura indicada no existe."
                );
            }

            if (request.Cantidad <= 0)
            {
                throw new ArgumentException(
                    "La cantidad debe ser mayor que cero."
                );
            }

            var tipoItem = request.TipoItem
                .Trim()
                .ToUpperInvariant();

            var descripcion = request.Descripcion;
            var precioUnitario = request.PrecioUnitario;

            switch (tipoItem)
            {
                case "MEDICAMENTO":
                    {
                        var medicamento =
                            await _tratamientoLabService.ObtenerMedicamentoAsync(
                                request.IdItem,
                                cancellationToken
                            );

                        if (medicamento == null)
                        {
                            throw new DomainException(
                                Errores.MEDICAMENTO_NO_EXISTE,
                                "El medicamento indicado no existe."
                            );
                        }

                        descripcion = medicamento.Descripcion;
                        precioUnitario = medicamento.Precio;

                        break;
                    }

                case "ESTUDIO":
                    {
                        var estudio =
                            await _tratamientoLabService.ObtenerEstudioAsync(
                                request.IdItem,
                                cancellationToken
                            );

                        if (estudio == null)
                        {
                            throw new DomainException(
                                Errores.ESTUDIO_NO_EXISTE,
                                "El estudio indicado no existe."
                            );
                        }

                        descripcion = estudio.Descripcion;
                        precioUnitario = estudio.Precio;

                        break;
                    }

                case "SERVICIO":
                    break;

                default:
                    throw new ArgumentException(
                        "TipoItem debe ser SERVICIO, MEDICAMENTO o ESTUDIO."
                    );
            }

            if (precioUnitario < 0)
            {
                throw new ArgumentException(
                    "El precio unitario no puede ser negativo."
                );
            }

            var detalle = new FacturaDetalle
            {
                IdFactura = request.IdFactura,
                TipoItem = tipoItem,
                IdItem = request.IdItem,
                Descripcion = descripcion,
                Cantidad = request.Cantidad,
                PrecioUnitario = precioUnitario
            };

            return await _repository.AddAsync(
                detalle,
                cancellationToken
            );
        }
    }
}