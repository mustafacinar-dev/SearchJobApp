using System.Security.Claims;
using MediatR;
using SearchJobApp.Application.Exceptions;
using SearchJobApp.Application.Helpers;
using SearchJobApp.Application.Interfaces.Helpers;
using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Application.Handlers;

public class GetAuthTokenQueryHandler : IRequestHandler<GetAuthTokenQuery, string>
{
    private readonly IEmployerRepository _employerRepository;
    private readonly IAuthTokenHelper _authTokenHelper;

    public GetAuthTokenQueryHandler(IEmployerRepository employerRepository, IAuthTokenHelper authTokenHelper)
    {
        _employerRepository = employerRepository;
        _authTokenHelper = authTokenHelper;
    }

    public async Task<string> Handle(GetAuthTokenQuery request, CancellationToken cancellationToken)
    {
        var employer = await _employerRepository.GetEmployerByPhoneOrEmail(null, request.Email);

        if (employer == null)
        {
            throw new EmployerNotFoundException();
        }

        if (!PasswordHasher.VerifyPassword(request.Password, employer.Password))
            throw new Exception("");

        return _authTokenHelper.GenerateToken(new[] { new Claim("employerId", employer.Id.ToString()) });
    }
}