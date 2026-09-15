using Generics.Interfaces;
using Generics.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        // =========================================
        // OBTENER TODOS
        // =========================================
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // =========================================
        // OBTENER POR ID
        // =========================================
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // =========================================
        // AGREGAR
        // =========================================
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        // =========================================
        // ACTUALIZAR
        // =========================================
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // =========================================
        // ELIMINAR
        // =========================================
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // =========================================
        // OBTENER PAGINADO
        // =========================================
        public async Task<PagedResult<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pageNumber),
                    "pageNumber debe ser mayor o igual a 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pageSize),
                    "pageSize debe ser mayor o igual a 1.");
            }

            IQueryable<T> query = _dbSet;

            // Solo lectura: mejora el rendimiento
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            // Incluir relaciones
            query = ApplyIncludes(query, includes);

            // Aplicar filtro
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Contar todos los registros que cumplen el filtro
            int totalRecords =
                await query.CountAsync(cancellationToken);

            // Aplicar ordenamiento
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Aplicar paginación
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Ejecutar consulta
            var data =
                await query.ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
        }

        // =========================================
        // APLICAR RELACIONES
        // =========================================
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