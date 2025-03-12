using YourWheel.Host.Middlewares;

namespace YourWheel.Host.Extensions
{
    /// <summary>
    /// Расширение для ExceptionMiddleware.
    /// </summary>
    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
