using SearchJobApp.Domain.Enums;

namespace SearchJobApp.Domain.Entities;

public class Post : BaseEntity
{
    public Guid EmployerId { get; set; }
    public string EmployerTitle { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public int? QualityScore { get; set; }
    public string? AdditionalMessage { get; set; }
    public WorkType? WorkType { get; set; }
    public PositionLevel? PositionLevel { get; set; }
    public string? Salary { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}