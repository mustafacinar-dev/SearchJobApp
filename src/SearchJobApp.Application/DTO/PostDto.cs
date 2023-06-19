namespace SearchJobApp.Application.DTO;

public class PostDto
{
    public Guid Id { get; set; }
    public Guid EmployerId { get; set; }
    public string EmployerTitle { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public int? QualityScore { get; set; }
    public string? AdditionalMessage { get; set; }
    public string? WorkType { get; set; }
    public string? PositionLevel { get; set; }
    public string? Salary { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}