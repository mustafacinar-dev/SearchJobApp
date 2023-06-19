namespace SearchJobApp.Application.DTO;

public class EmployerWithPostsDto : EmployerDto
{
    public IEnumerable<PostDto> Posts { get; set; }
}