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

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _mediator.Send(new GetAllEmployersQuery()));
    }

    [HttpGet]
    [Route("{employerId}")]
    public async Task<IActionResult> GetEmployerAsync(string employerId)
    {
        return Ok(await _mediator.Send(new GetEmployerQuery() { EmployerId = Guid.Parse(employerId) }));
    }
    
    [HttpGet]
    [Route("{employerId}/posts")]
    public async Task<IActionResult> GetEmployerWithPostsAsync(string employerId)
    {
        return Ok(await _mediator.Send(new GetEmployerWithPostsQuery { EmployerId = Guid.Parse(employerId) }));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateEmployerCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}