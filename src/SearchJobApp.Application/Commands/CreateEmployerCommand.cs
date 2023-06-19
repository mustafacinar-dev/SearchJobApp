using MediatR;

namespace SearchJobApp.Application.Commands;

public class CreateEmployerCommand : IRequest<Guid>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}