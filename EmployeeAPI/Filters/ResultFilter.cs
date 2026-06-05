using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace EmployeeAPI.Filters
{
    public class ResultFilter: ResultFilterAttribute
    {
        private readonly ILogger<ResultFilter> _logger;
        public ResultFilter(ILogger<ResultFilter> logger)
        {
            _logger = logger;
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.StatusCode = 400;
            // Code to execute before the action result is executed
            // For example, you can modify the result or perform logging tasks here
            _logger.LogInformation("Result filter executing");
            context.HttpContext.Response.Headers["Company"] = "EmployeeAPI";
            context.Result = new JsonResult(new
            {
                Success = false,
                Message = "Response changed by Result Filter"
            })
            {
                StatusCode = 500
            };


            base.OnResultExecuting(context);

        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            // Code to execute after the action result has been executed
            // For example, you can perform cleanup tasks or modify the response here
            _logger.LogInformation("Result filter executed");
            _logger.LogInformation("Executed: {ActionName}", context.ActionDescriptor.DisplayName);

            base.OnResultExecuted(context);
        }
    }
}
