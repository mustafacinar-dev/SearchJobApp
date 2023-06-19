using MediatR;
using SearchJobApp.Application.DTO;

namespace SearchJobApp.Application.Queries;

public class GetPostsByEmployerIdQuery : IRequest<IEnumerable<PostDto>>
{
    public Guid EmployerId { get; set; }
}