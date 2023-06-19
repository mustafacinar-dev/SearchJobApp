using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login(GetAuthTokenQuery query)
    {
        var accessToken = await _mediator.Send(query);
        return Ok(new { accessToken });
    }
}