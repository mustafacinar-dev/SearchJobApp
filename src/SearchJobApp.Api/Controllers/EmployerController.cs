using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchJobApp.Application.Commands;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployerController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateEmployerCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _mediator.Send(new GetAllEmployersQuery()));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        return Ok(await _mediator.Send(new GetEmployerQuery() { EmployerId = Guid.Parse(id) }));
    }

    [HttpGet]
    [Route("{id}/posts")]
    public async Task<IActionResult> GetEmployerWithPostsAsync(string id)
    {
        return Ok(await _mediator.Send(new GetEmployerWithPostsQuery { EmployerId = Guid.Parse(id) }));
    }
}