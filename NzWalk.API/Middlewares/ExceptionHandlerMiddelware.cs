using System.Net;

namespace NzWalk.API.Middlewares
    {
    public class ExceptionHandlerMiddelware
        {
        // Field to hold the logger instance
        private readonly ILogger<ExceptionHandlerMiddelware> logger;

        // Field to hold the next middleware in the pipeline
        private readonly RequestDelegate next;

        // Constructor to initialize the logger and next fields
        public ExceptionHandlerMiddelware(ILogger<ExceptionHandlerMiddelware> logger, RequestDelegate next)
            {
            this.logger = logger; // Assign the logger instance
            this.next = next;     // Assign the next middleware
            }

        // Method to handle requests and catch exceptions
        public async Task InvokeAsync(HttpContext httpContext)
            {
            try
                {
                // Pass the request to the next middleware in the pipeline
                await next(httpContext);
                }
            catch (Exception ex)
                {
                // Generate a unique error ID for tracking
                var errorId = Guid.NewGuid();

                // Log the exception details along with the error ID
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Set the HTTP status code to Internal Server Error
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // Set the content type to JSON
                httpContext.Response.ContentType = "application/json";

                // Create an anonymous object to represent the error response
                var error = new
                    {
                    Id = errorId,                 
                    ErrorMessage = "Something went wrong" 
                    };

                // Write the error response as JSON
                await httpContext.Response.WriteAsJsonAsync(error);
                }
            }
        }
    }
