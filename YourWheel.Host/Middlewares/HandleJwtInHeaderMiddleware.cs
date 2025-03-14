namespace YourWheel.Host.Middlewares
{
    using YourWheel.Host.Extensions;
    using YourWheel.Host.Services;

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

        public async Task InvokeAsync(HttpContext httpContext, IAuthenticationService authService,
            IJwtService jwtService)
        {
            // Получаю токен из cookie
            var tokenString = httpContext.ReadToken();

            // Валидируем токен
            if (jwtService.IsTokenStringValid(tokenString))
            {
                // Срок годности токена
                var tokenExpiryDate = jwtService.GetTokenExpiryDateTime(tokenString);

                var now = DateTime.UtcNow;

                var timeDiff = tokenExpiryDate.Subtract(now).TotalMinutes;

                if (timeDiff < TimeSpan.FromMinutes(1).TotalMinutes)
                {
                    var userId = jwtService.GetUserId(tokenString);

                    // Взять из БД по идентификатору !!! - не реализовано
                    var identity = await authService.GetIdentityAsync(userId);

                    if (identity != null)
                    {
                        var jwt = jwtService.GenerateToken(identity.Claims);

                        tokenString = jwtService.GetTokenString(jwt);

                        httpContext.AddToken(tokenString);
                    }
                }

                httpContext.Request.Headers.Append("Authorization", "Bearer " + tokenString);
            }

            await _next.Invoke(httpContext);
        }
    }
}
