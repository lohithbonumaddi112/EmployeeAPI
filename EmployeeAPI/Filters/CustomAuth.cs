using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Filters
{
    public class CustomAuth: IAuthorizationFilter   
    {
        public void OnAuthorization(AuthorizationFilterContext context) {

            if (!context.HttpContext.User.IsInRole("Root")) { 
            
                context.Result= new UnauthorizedResult();
            }

        }
    }
}
