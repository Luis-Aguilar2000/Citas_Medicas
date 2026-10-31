using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generics.Interfaces;
using Generics.Models;
using Microsoft.EntityFrameworkCore;

namespace Generics.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
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

    public async Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public Task<T?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return GetByIdAsync(
            new object[] { id },
            cancellationToken);
    }

    public async Task<T?> GetByIdAsync(
        object[] keyValues,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(keyValues);

        var primaryKey = _dbSet.EntityType.FindPrimaryKey()
            ?? throw new InvalidOperationException(
                $"La entidad {typeof(T).Name} no tiene " +
                "clave primaria configurada.");

        if (keyValues.Length != primaryKey.Properties.Count)
        {
            throw new ArgumentException(
                $"La entidad {typeof(T).Name} requiere " +
                $"{primaryKey.Properties.Count} valor(es) de clave.",
                nameof(keyValues));
        }

        for (int i = 0; i < keyValues.Length; i++)
        {
            var property = primaryKey.Properties[i];
            var expectedType = property.ClrType;

            if (keyValues[i] is null ||
                !expectedType.IsInstanceOfType(keyValues[i]))
            {
                throw new ArgumentException(
                    $"La clave {property.Name} debe ser " +
                    $"de tipo {expectedType.Name}.",
                    nameof(keyValues));
            }
        }

        return await _dbSet.FindAsync(
            keyValues,
            cancellationToken);
    }

    public async Task<T?> GetOneByAsync(
        Expression<Func<T, bool>> filter,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return await Query(asNoTracking)
            .FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<T> AddAsync(
        T entity,
        CancellationToken cancellationToken = default)
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

    public async Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        T entity,
        CancellationToken cancellationToken = default)
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

        var query = ApplyIncludes(
            Query(asNoTracking),
            includes);

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (splitQuery)
        {
            query = query.AsSplitQuery();
        }

        int totalRecords = await query.CountAsync(
            cancellationToken);

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        int skip = checked((pageNumber - 1) * pageSize);

        var data = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Data = data,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(
                (double)totalRecords / pageSize),
            CurrentPage = pageNumber,
            PageSize = pageSize
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