namespace YourWheel.Host.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net;
    using YourWheel.Domain.Dto;
    using YourWheel.Host.Extensions;
    using YourWheel.Host.Logging;

    /// <summary>
    /// Middleware для логирования и обработки ошибок.
    /// </summary>
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception exception, string messageError = "Internal Server Error",
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = (int)statusCode;

            var result = new DetailsDto() 
            {
                Details = messageError
            }.ToString();

            var token = httpContext.ReadToken();

            Guid userId = Guid.Empty;

            if (!string.IsNullOrEmpty(token))
            {
                string userIdString = new JwtSecurityToken(token)?.Payload[Constants.UserIdClaimType]?.ToString();

                Guid.TryParse(userIdString, out userId);
            }

            Log.Error(exception.StackTrace, userId);

            return httpContext.Response.WriteAsync(result);
        }
    }
}
