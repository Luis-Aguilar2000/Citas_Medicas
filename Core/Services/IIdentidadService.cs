namespace Core.Services
{
    public interface IIdentidadService
    {
        Task<bool> MedicoValidoAsync(
            int idMedico,
            int idEspecialidad,
            CancellationToken cancellationToken = default);
    }
}