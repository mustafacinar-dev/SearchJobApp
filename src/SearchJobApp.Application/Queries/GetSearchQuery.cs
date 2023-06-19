using MediatR;
using SearchJobApp.Application.DTO;

namespace SearchJobApp.Application.Queries;

public class GetSearchQuery : IRequest<IEnumerable<PostDto>>
{
    public string? SearchText { get; set; }
    public string? WorkType { get; set; }
    public string? PositionLevel { get; set; }
}