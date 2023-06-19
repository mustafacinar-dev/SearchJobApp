using SearchJobApp.Application.Queries;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Interfaces.Services;

public interface IPostElasticSearchService
{
    Task InsertAsync(Post post);
    Task UpdateAsync(Guid id, Post post);
    Task<Post> GetAsync(Guid id);
    Task<IEnumerable<Post>> GetAllAsync();
    Task<IEnumerable<Post>> GetPostsByEmployerId(string employerId);
    Task<IEnumerable<Post>> SearchAsync(GetSearchQuery searchQuery);
    Task RemoveAsync(Guid id);
}