using Generics.Models;
using System.Linq.Expressions;

namespace Generics.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<PagedResult<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);


       
    }
}