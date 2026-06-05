using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeAPI.Filters
{
    public class CustExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            // Code to execute when an exception occurs
            // For example, you can log the exception or return a custom error response here
            context.Result= new ObjectResult(new
            {
                Message = "An error occurred while processing your request.",
                Details = context.Exception.Message
            })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
            base.OnException(context);
        }
    }
}
