using Microsoft.AspNetCore.Http;
using SearchJobApp.Application.Interfaces.Helpers;

namespace SearchJobApp.Application.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IAuthTokenHelper authTokenHelper)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null) AddUserToContext(context, authTokenHelper, token);

        await _next(context);
    }

    private void AddUserToContext(HttpContext context, IAuthTokenHelper authTokenHelper, string token)
    {
        try
        {
            var jwtToken = authTokenHelper.ValidateToken(token);
            var employerId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "employerId").Value);

            context.Items["EmployerId"] = employerId;
        }
        catch
        {
            // ignored
        }
    }
}