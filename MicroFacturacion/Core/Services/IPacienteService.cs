using System.Threading;
using System.Threading.Tasks;

namespace Core.Facturacion.Services
{
    public interface IPacienteService
    {
        Task<bool> ExisteAsync(
            int idPaciente,
            CancellationToken cancellationToken = default);
    }
}