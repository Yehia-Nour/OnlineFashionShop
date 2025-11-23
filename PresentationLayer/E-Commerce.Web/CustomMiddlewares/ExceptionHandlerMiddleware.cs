using ECommerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.CustomMiddlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                await HandelNotFoundEndpointAsync(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went Wrong");
                var problem = new ProblemDetails
                {
                    Title = "Error While Processing HTTP Request",
                    Detail = ex.Message,
                    Instance = httpContext.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };
                httpContext.Response.StatusCode = problem.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problem);
            }
        }

        private static async Task HandelNotFoundEndpointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var problem = new ProblemDetails
                {
                    Title = "Error While Processing the HTTP Request - Endpiont Not Found",
                    Detail = $"Endpint {httpContext.Request.Path} Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Instance = httpContext.Request.Path
                };
                await httpContext.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
