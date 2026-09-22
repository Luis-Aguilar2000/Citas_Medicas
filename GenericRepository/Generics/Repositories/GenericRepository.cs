using System.Linq.Expressions;
using Generics.Interfaces;
using Generics.Models;
using Microsoft.EntityFrameworkCore;

namespace Generics.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        private IQueryable<T> Query(bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<T?> GetOneByAsync(
            Expression<Func<T, bool>> filter,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            IQueryable<T> query = Query(asNoTracking);

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            var lista = entities.ToList();

            if (lista.Count == 0)
            {
                return lista;
            }

            await _dbSet.AddRangeAsync(lista, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return lista;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true,
            bool splitQuery = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "pageNumber debe ser mayor o igual a 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "pageSize debe ser mayor o igual a 1.");
            }

            var query = ApplyIncludes(Query(asNoTracking), includes);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (splitQuery)
            {
                query = query.AsSplitQuery();
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var data = await query.ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Data = data,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
        }

        private static IQueryable<T> ApplyIncludes(
            IQueryable<T> query,
            Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
    }
}