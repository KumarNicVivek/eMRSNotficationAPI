using EmrsAppAdminService.CommonModel;
using System.Net;
using System.Text.Json;

namespace EmrsAppAdminService.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex, httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>());                
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception,IWebHostEnvironment env)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.FailureResponse("An unexpected error occurred. Please try again later.");

            if(env.IsDevelopment())
            {
                response.Message = exception.Message;
            }

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
           
        }
    }
}
