using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchJobApp.Api.Attributes;
using SearchJobApp.Application.Commands;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostAsync(CreatePostCommand request)
    {
        Guid.TryParse(HttpContext.Items["EmployerId"]?.ToString(), out Guid employerId);
        request.EmployerId = employerId;

        return Created(string.Empty, await _mediator.Send(request));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _mediator.Send(new GetAllPostsQuery()));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        return Ok(await _mediator.Send(new GetPostQuery { PostId = Guid.Parse(id) }));
    }

    [HttpGet]
    [Route("employer")]
    public async Task<IActionResult> GetPostsByEmployerIdAsync(string employerId)
    {
        return Ok(await _mediator.Send(new GetPostsByEmployerIdQuery() { EmployerId = Guid.Parse(employerId) }));
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> GetSearchAsync(string? searchText = null, string? workType = null,
        string? positionLevel = null)
    {
        return Ok(await _mediator.Send(new GetSearchQuery()
            { SearchText = searchText, PositionLevel = positionLevel, WorkType = workType }));
    }
}