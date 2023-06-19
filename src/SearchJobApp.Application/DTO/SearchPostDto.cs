namespace SearchJobApp.Application.DTO;

public class SearchPostDto
{
    public string Title { get; set; }
    public string Message { get; set; }
    public int? QualityScore { get; set; }
    public string? AdditionalMessage { get; set; }
    public string? WorkType { get; set; }
    public string? PositionLevel { get; set; }
    public string? Salary { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid CompanyId { get; set; }
}