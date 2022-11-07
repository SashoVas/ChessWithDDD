using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                ProblemDetails problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Invalid request",
                    Type = "Client error",
                    Detail = e.Message
                };
                string json=JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);
                context.Response.ContentType = "application/json";
            }
            
        }
    }
}
