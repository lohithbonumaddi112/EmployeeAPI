using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeAPI.Filters
{
    public class ResourceFilter: IResourceFilter
    {
      
        private readonly ILogger<ResourceFilter> _logger;
        public ResourceFilter(ILogger<ResourceFilter> logger)
        {
            _logger = logger;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Code to execute before the resource is executed
            // For example, you can perform authentication or logging tasks here
            _logger.LogInformation("Resource filter executing");
            var block = context.HttpContext.Request.Query["block"];
            if (block == "true")
            {
                context.HttpContext.Response.StatusCode = 403; // Forbidden
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
                {
                    Message = "Access to this resource is blocked."
                });
            }
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Code to execute after the resource has been executed
            // For example, you can perform cleanup tasks or modify the response here
           _logger.LogInformation("Resource filter executed");
        }
    }
}
