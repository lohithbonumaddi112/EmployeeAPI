namespace EmployeeAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) {
           
            try
            {
                _logger.LogInformation("ExceptionMiddleware invoked");
                _logger.LogInformation("Processing request: {Method} {Path}", context.Request.Method, context.Request.Path);

                await _next(context);
            }
            catch (Exception e)
            {
               _logger.LogError(e, "An unhandled exception occurred while processing the request.");
               _logger.LogInformation("Exception details: {Message}", e.Message);
                await Handler(context);
                return;
            }
        }
        public static Task Handler(HttpContext context) { 
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
          
            return context.Response.WriteAsJsonAsync(new { 
            Error= $"An unexpected error occurred. Please try again later. "

            } );

        }
    }
}
