using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SearchJobApp.Application.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var result = JsonSerializer.Serialize(new
                { success = false, exception.Message });
            await response.WriteAsync(result);
        }
    }
}