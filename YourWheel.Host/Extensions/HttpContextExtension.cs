namespace YourWheel.Host.Extensions
{
    /// <summary>
    ///   Расширения для HttpContext
    /// </summary>
    public static class HttpContextExtension
    {
        private const string YOURWHEEL_TOKEN_KEY = ".YourWheel.token";

        /// <summary>
        /// Выбранный пользоввателем язык
        /// </summary>
        public const string YOURWHEEL_CULTURE = ".YourWheel.culture";

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

        /// <summary>
        /// Добавление значения по ключу в куки
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        public static void AddCookieKey(this HttpContext context, string key, string value)
        {
            context.Response.Cookies.Append(key, value,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(30)
                });
        }

        /// <summary>
        ///   Удаление по ключу из cookies
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="key">Удаляемый ключ</param>
        public static void DeleteCookieByKey(this HttpContext context, string key)
        {
            context.Response.Cookies.Delete(key);
        }

        /// <summary>
        ///   Чтение по ключу из cookies
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="key">Запрашиваемый ключ</param>
        /// <returns>Строка со значением по ключу</returns>
        public static string ReadCookieByKey(this HttpContext context, string key)
        {
            return context.Request.Cookies[key];
        }
    }
}
