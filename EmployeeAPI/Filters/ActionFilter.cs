using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeAPI.Controllers;

namespace EmployeeAPI.Filters
{
    public class ActionFilter: ActionFilterAttribute
    {
        
        public Stopwatch? stopwatch;
        private readonly ILogger _logger;

        public ActionFilter(ILogger<ActionFilter> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
            {
                // Code to execute before the action method is called
                // For example, you can log the request or validate input here
                _logger.LogInformation("Action Filter executing");
            _logger.LogInformation("Executing: {ActionName}", context.ActionDescriptor.DisplayName);
            base.OnActionExecuting(context);
            stopwatch = Stopwatch.StartNew();

            }
            public override void OnActionExecuted(ActionExecutedContext context)
            {
            // Code to execute after the action method is called
            // For example, you can log the response or handle exceptions here
            _logger.LogInformation("Action Filter executed");
            _logger.LogInformation("Executed: {ActionName}", context.ActionDescriptor.DisplayName);
            base.OnActionExecuted(context);
            stopwatch!.Stop();
            _logger.LogInformation("Ended");
            _logger.LogInformation("Time taken {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
           

        }
        

    }
}
