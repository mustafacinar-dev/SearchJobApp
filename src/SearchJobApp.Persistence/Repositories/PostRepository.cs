using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Domain.Entities;
using SearchJobApp.Persistence.Context;

namespace SearchJobApp.Persistence.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(SearchJobAppDbContext dbContext) : base(dbContext)
    {
    }
}