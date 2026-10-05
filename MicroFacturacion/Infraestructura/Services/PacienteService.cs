using Core.Facturacion.Configs;
using Core.Facturacion.Services;
using Core.Facturacion.Exceptions;
using Microsoft.Extensions.Options;
using NugetPackage_Rest.Exceptions;
using NugetPackage_Rest.Interfaces.IServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructura.Services
{
    internal class PacienteService : IPacienteService
    {
        private readonly IRest _rest;
        private readonly DownstreamOptions _options;

        public PacienteService(
            IRest rest,
            IOptions<DownstreamOptions> options)
        {
            _rest = rest;
            _options = options.Value;
        }

        public async Task<bool> ExisteAsync(
            int idPaciente,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (idPaciente <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(
                _options.PacientesAgendasBaseUrl))
            {
                throw new InvalidOperationException(
                    "Falta configurar Downstream:PacientesAgendasBaseUrl.");
            }

            try
            {
                await _rest.Get
                    .WithoutAuth()
                    .WithUri(
                        _options.PacientesAgendasBaseUrl.TrimEnd('/'),
                        $"/Pacientes/{idPaciente}")
                    .GetContentAsStringAsync();

                return true;
            }
            catch (ApiException ex)
                when (ex.Reason == ApiFailureReason.HttpError &&
                      ex.StatusCode == 404)
            {
                return false;
            }
            catch (ApiException ex)
                when (ex.Reason == ApiFailureReason.Network ||
                      ex.Reason == ApiFailureReason.Timeout)
            {
                throw new DomainException(
                    Errores.PACIENTES_NO_DISPONIBLE,
                    "No se pudo consultar PacientesAgendas. Intenta nuevamente más tarde."
                );
            }
        }
    }
}