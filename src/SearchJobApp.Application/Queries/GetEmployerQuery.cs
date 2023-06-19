using MediatR;
using SearchJobApp.Application.DTO;

namespace SearchJobApp.Application.Queries;

public class GetEmployerQuery : IRequest<EmployerDto>
{
    public Guid EmployerId { get; set; }
}