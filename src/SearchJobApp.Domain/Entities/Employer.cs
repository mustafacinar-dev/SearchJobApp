namespace SearchJobApp.Domain.Entities;

public class Employer : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int RemainingPostingQuantity { get; set; }
}