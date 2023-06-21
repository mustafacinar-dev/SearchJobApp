using MediatR;
using SearchJobApp.Application.DTO;

namespace SearchJobApp.Application.Queries;

public class GetPostQuery : IRequest<PostDto>
{
    public Guid PostId { get; set; }
}