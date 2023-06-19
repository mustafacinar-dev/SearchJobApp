using System.Linq.Expressions;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T: BaseEntity
{
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetById(Guid id);
}