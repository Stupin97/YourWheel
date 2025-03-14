namespace YourWheel.Host.Extensions
{
    /// <summary>
    ///   Расширения для HttpContext
    /// </summary>
    public static class HttpContextExtension
    {
        private const string YOURWHEEL_TOKEN_KEY = ".YourWheel.token";

        /// <summary>
        ///   Добавление заголовков для безопасного обмена jwt в куках
        /// </summary>
        /// <param name="context">HttpContext</param>
        public static void AddSecurityHeaders(this HttpContext context)
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Xss-Protection", "1");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
        }

        /// <summary>
        ///   Добавление строки с токеном в куки
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="tokenString">Строка с токеном</param>
        public static void AddToken(this HttpContext context, string tokenString)
        {
            context.Response.Cookies.Append(YOURWHEEL_TOKEN_KEY, tokenString,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(3)
                });
        }


        /// <summary>
        ///   Удаление jwt из кук
        /// </summary>
        /// <param name="context">HttpContext</param>
        public static void DeleteToken(this HttpContext context)
        {
            context.Response.Cookies.Delete(YOURWHEEL_TOKEN_KEY);
        }

        /// <summary>
        ///   Чтение jwt из кук
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Строка с токеном</returns>
        public static string ReadToken(this HttpContext context)
        {
            return context.Request.Cookies[YOURWHEEL_TOKEN_KEY];
        }
    }
}
