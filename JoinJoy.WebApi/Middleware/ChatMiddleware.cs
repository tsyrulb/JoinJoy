using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Middleware
{
    public class ChatMiddleware
    {
        private readonly RequestDelegate _next;

        public ChatMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Implement your chat handling logic here

            await _next(context);
        }
    }
}
