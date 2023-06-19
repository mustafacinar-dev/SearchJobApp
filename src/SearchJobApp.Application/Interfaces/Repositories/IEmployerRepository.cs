using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Interfaces.Repositories;

public interface IEmployerRepository: IGenericRepository<Employer>
{
    Task<Employer?> GetEmployerByPhoneOrEmail(string phone, string email);
}