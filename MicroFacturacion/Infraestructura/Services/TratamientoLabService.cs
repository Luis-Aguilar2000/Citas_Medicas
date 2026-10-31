using Core.Facturacion.Configs;
using Core.Facturacion.Exceptions;
using Core.Facturacion.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NugetPackage_Rest.Exceptions;
using NugetPackage_Rest.Interfaces.IServices;

namespace Infraestructura.Services
{
    internal class TratamientoLabService : ITratamientoLabService
    {
        private readonly IRest _rest;
        private readonly DownstreamOptions _options;

        public TratamientoLabService(
            IRest rest,
            IOptions<DownstreamOptions> options)
        {
            _rest = rest;
            _options = options.Value;
        }

        public async Task<ItemTratamientoDto?> ObtenerMedicamentoAsync(
            int idMedicamento,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (idMedicamento <= 0)
                return null;

            ValidarConfiguracion();

            try
            {
                var contenido = await _rest.Get
                    .WithoutAuth()
                    .WithUri(
                        _options.TratamientosLabBaseUrl.TrimEnd('/'),
                        $"/Medicamento/{idMedicamento}")
                    .GetContentAsStringAsync();

                var medicamento =
                    JsonConvert.DeserializeObject<MedicamentoResponse>(contenido);

                if (medicamento == null)
                    return null;

                return new ItemTratamientoDto
                {
                    IdItem = medicamento.IdMedicamento,
                    Descripcion = medicamento.NombreComercial,
                    Precio = medicamento.Precio
                };
            }
            catch (ApiException ex)
                when (ex.Reason == ApiFailureReason.HttpError &&
                      ex.StatusCode == 404)
            {
                return null;
            }
            catch (ApiException ex)
                when (ex.Reason == ApiFailureReason.Network ||
                      ex.Reason == ApiFailureReason.Timeout)
            {
                throw new DomainException(
                    Errores.TRATAMIENTOS_LAB_NO_DISPONIBLE,
                    "No se pudo consultar Tratamientos-Lab. Intenta nuevamente más tarde."
                );
            }
        }

        public async Task<ItemTratamientoDto?> ObtenerEstudioAsync(
            int idEstudio,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (idEstudio <= 0)
                return null;

            ValidarConfiguracion();

            try
            {
                var contenido = await _rest.Get
                    .WithoutAuth()
                    .WithUri(
                        _options.TratamientosLabBaseUrl.TrimEnd('/'),
                        $"/CatalogoEstudio/{idEstudio}")
                    .GetContentAsStringAsync();

                var estudio =
                    JsonConvert.DeserializeObject<CatalogoEstudioResponse>(contenido);

                if (estudio == null)
                    return null;

                return new ItemTratamientoDto
                {
                    IdItem = estudio.IdCatalogoEstudio,
                    Descripcion = estudio.NombreEstudio,
                    Precio = estudio.Precio
                };
            }
            catch (ApiException ex)
                when (ex.Reason == ApiFailureReason.HttpError &&
                      ex.StatusCode == 404)
            {
                return null;
            }
            catch (ApiException ex)
                when (ex.Reason == ApiFailureReason.Network ||
                      ex.Reason == ApiFailureReason.Timeout)
            {
                throw new DomainException(
                    Errores.TRATAMIENTOS_LAB_NO_DISPONIBLE,
                    "No se pudo consultar Tratamientos-Lab. Intenta nuevamente más tarde."
                );
            }
        }

        private void ValidarConfiguracion()
        {
            if (string.IsNullOrWhiteSpace(
                _options.TratamientosLabBaseUrl))
            {
                throw new InvalidOperationException(
                    "Falta configurar Downstream:TratamientosLabBaseUrl."
                );
            }
        }

        private class MedicamentoResponse
        {
            public int IdMedicamento { get; set; }
            public string NombreComercial { get; set; } = string.Empty;
            public decimal Precio { get; set; }
        }

        private class CatalogoEstudioResponse
        {
            public int IdCatalogoEstudio { get; set; }
            public string NombreEstudio { get; set; } = string.Empty;
            public decimal Precio { get; set; }
        }
    }
}