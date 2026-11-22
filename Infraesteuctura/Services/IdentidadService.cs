using Core.Configs;
using Core.Services;
using Microsoft.Extensions.Options;
using NugetPackage_Rest.Exceptions;
using NugetPackage_Rest.Interfaces.IServices;
using System.Text.Json;

namespace Infraestructura.Services
{
    public class IdentidadService : IIdentidadService
    {
        private readonly IRest _rest;
        private readonly DownstreamOptions _options;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public IdentidadService(
            IRest rest,
            IOptions<DownstreamOptions> options)
        {
            _rest = rest;
            _options = options.Value;
        }

        public async Task<bool> MedicoValidoAsync(
            int idMedico,
            int idEspecialidad,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (idMedico <= 0 || idEspecialidad <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(
                _options.IdentidadUsuariosBaseUrl))
            {
                throw new InvalidOperationException(
                    "Falta configurar Downstream:IdentidadUsuariosBaseUrl.");
            }

            var baseUrl =
                _options.IdentidadUsuariosBaseUrl.TrimEnd('/');

            try
            {
                var medicoJson = await _rest.Get
                    .WithoutAuth()
                    .WithUri(
                        baseUrl,
                        $"/Medicos/{idMedico}")
                    .GetContentAsStringAsync();

                var medico =
                    JsonSerializer.Deserialize<MedicoResponse>(
                        medicoJson,
                        JsonOptions);

                if (medico == null || !medico.Estado)
                    return false;

                var relacionJson = await _rest.Get
                    .WithoutAuth()
                    .WithUri(
                        baseUrl,
                        $"/MedicoEspecialidades/{idMedico}/{idEspecialidad}")
                    .GetContentAsStringAsync();

                var relacion =
                    JsonSerializer.Deserialize<MedicoEspecialidadResponse>(
                        relacionJson,
                        JsonOptions);

                return relacion != null &&
                       relacion.IdMedico == idMedico &&
                       relacion.IdEspecialidad == idEspecialidad;
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
                throw new InvalidOperationException(
                    "MicroIdentidadUsuarios no está disponible.",
                    ex);
            }
        }

        private class MedicoResponse
        {
            public int IdMedico { get; set; }
            public bool Estado { get; set; }
        }

        private class MedicoEspecialidadResponse
        {
            public int IdMedico { get; set; }
            public int IdEspecialidad { get; set; }
        }
    }
}