using MediatR;
using SearchJobApp.Domain.Enums;

namespace SearchJobApp.Application.Commands;

public class CreatePostCommand : IRequest<Guid>
{
    public Guid EmployerId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string? AdditionalMessage { get; set; }
    public WorkType? WorkType { get; set; }
    public PositionLevel? PositionLevel { get; set; }
    public string? Salary { get; set; }
}