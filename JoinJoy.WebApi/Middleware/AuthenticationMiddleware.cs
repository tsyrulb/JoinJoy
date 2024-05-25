// File: JoinJoy.WebApi/Middleware/AuthenticationMiddleware.cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Authentication logic here
        await _next(context);
    }
}
