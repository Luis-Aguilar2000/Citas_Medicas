using System.Linq.Expressions;
using Generics.Models;

namespace Generics.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // CRUD Básicos
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        // Consultas y Filtros
        Task<T?> GetOneByAsync(
            Expression<Func<T, bool>> filter,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default
        );

        // Operaciones en Lote
        Task<IEnumerable<T>> AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default
        );

        // Paginación Avanzada
        Task<PagedResult<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true,
            bool splitQuery = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes
        );
    }
}