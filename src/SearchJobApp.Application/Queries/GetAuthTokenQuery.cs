using MediatR;

namespace SearchJobApp.Application.Queries;

public class GetAuthTokenQuery : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}