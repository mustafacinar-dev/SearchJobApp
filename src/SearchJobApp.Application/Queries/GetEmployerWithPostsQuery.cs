using MediatR;
using SearchJobApp.Application.DTO;

namespace SearchJobApp.Application.Queries;

public class GetEmployerWithPostsQuery : IRequest<EmployerWithPostsDto>
{
    public Guid EmployerId { get; set; }
}