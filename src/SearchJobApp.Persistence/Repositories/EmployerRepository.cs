using Microsoft.EntityFrameworkCore;
using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Domain.Entities;
using SearchJobApp.Persistence.Context;

namespace SearchJobApp.Persistence.Repositories;

public class EmployerRepository : GenericRepository<Employer>, IEmployerRepository
{
    public EmployerRepository(SearchJobAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Employer?> GetEmployerByPhoneOrEmail(string? phone, string? email) =>
        await _dbContext
            .Set<Employer>().AsNoTracking()
            .FirstOrDefaultAsync(e => e.Phone == phone || e.Email == email);
}