using System.Net;

namespace NzWalk.API.Middlewares
    {
    public class ExceptionHandlerMiddelware
        {
        private readonly ILogger<ExceptionHandlerMiddelware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddelware(ILogger<ExceptionHandlerMiddelware> logger, RequestDelegate next)
            {
            this.logger = logger;
            this.next = next;
            }

        public async Task InvokeAsync(HttpContext httpContext)
            {
            try
                {
                await next(httpContext);
                }
            catch (Exception ex)
                {
                var errorId = Guid.NewGuid();
                // log the error message
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return a custome error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                    {
                    Id = errorId,
                    ErrorMessage = "somethig went wrong"
                    };

                await httpContext.Response.WriteAsJsonAsync(error);
                }
            }
        }
    }
