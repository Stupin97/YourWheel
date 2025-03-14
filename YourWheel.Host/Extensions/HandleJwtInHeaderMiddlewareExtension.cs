using YourWheel.Host.Middlewares;

namespace YourWheel.Host.Extensions
{
    /// <summary>
    ///   Расширение для HandleJwtInHeaderMiddleware.
    /// </summary>
    public static class HandleJwtInHeaderMiddlewareExtension
    {
        public static IApplicationBuilder UseHandleJwtInHeaderMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<HandleJwtInHeaderMiddleware>();
        }
    }
}
