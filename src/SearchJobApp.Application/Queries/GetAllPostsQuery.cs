using MediatR;
using SearchJobApp.Application.DTO;

namespace SearchJobApp.Application.Queries;

public class GetAllPostsQuery : IRequest<IEnumerable<PostDto>>
{
}