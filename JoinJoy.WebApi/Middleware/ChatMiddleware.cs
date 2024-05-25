// File: JoinJoy.WebApi/Middleware/ChatMiddleware.cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class ChatMiddleware
{
    private readonly RequestDelegate _next;

    public ChatMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Chat handling logic
        await _next(context);
    }
}
