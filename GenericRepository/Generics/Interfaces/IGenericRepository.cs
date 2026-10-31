using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generics.Models;

namespace Generics.Interfaces;

public interface IGenericRepository<T> where T : class
{
    // Consultar todos los registros.
    Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default);

    // Consultar por una clave int.
    Task<T?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    // Consultar por Guid, long o claves compuestas.
    Task<T?> GetByIdAsync(
        object[] keyValues,
        CancellationToken cancellationToken = default);

    // Crear un registro.
    Task<T> AddAsync(
        T entity,
        CancellationToken cancellationToken = default);

    // Actualizar un registro.
    Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default);

    // Eliminar un registro.
    Task DeleteAsync(
        T entity,
        CancellationToken cancellationToken = default);

    // Consultar un registro mediante una condición.
    Task<T?> GetOneByAsync(
        Expression<Func<T, bool>> filter,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    // Crear varios registros.
    Task<IEnumerable<T>> AddRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default);

    // Consultar registros con paginación, filtros y relaciones.
    Task<PagedResult<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        bool splitQuery = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);
}