using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Interfaces.Services;

public interface IEmployerElasticSearchService
{
    Task InsertAsync(Employer employer);
    Task UpdateAsync(Guid id, Employer employer);
    Task<Employer> GetAsync(Guid id);
    Task<IEnumerable<Employer>> GetAllAsync();
    Task RemoveAsync(Guid id);
}