using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Domain.Entities;
using SearchJobApp.Persistence.Context;

namespace SearchJobApp.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly SearchJobAppDbContext _dbContext;

    public GenericRepository(SearchJobAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task RemoveAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetById(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
}