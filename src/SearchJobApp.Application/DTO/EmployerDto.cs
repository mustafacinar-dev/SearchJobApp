namespace SearchJobApp.Application.DTO;

public class EmployerDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int RemainingPostingQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}