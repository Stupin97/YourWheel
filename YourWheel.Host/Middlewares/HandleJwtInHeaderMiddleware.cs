using Microsoft.AspNetCore.Authentication;

namespace YourWheel.Host.Middlewares
{
    /// <summary>
    ///   Middleware для переноса Jwt из кук в header запроса
    /// </summary>
    public class HandleJwtInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleJwtInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync()
        {
            // ОСТАНОВИЛСЯ ТУТ, подумать, как оптимально сделать
        }
    }
}
