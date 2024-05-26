using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Implement your authentication logic here

            await _next(context);
        }
    }
}
