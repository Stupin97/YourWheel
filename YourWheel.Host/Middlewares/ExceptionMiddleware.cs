namespace YourWheel.Host.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using System.Net;
    using YourWheel.Domain.Dto;

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
                await HandleExceptionAsync(context);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, string messageError = "Internal Server Error",
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = (int)statusCode;

            var result = new DetailsDto() 
            {
                Details = messageError
            }.ToString();

            return httpContext.Response.WriteAsync(result);
        }
    }
}
