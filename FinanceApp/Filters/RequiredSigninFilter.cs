using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FinanceApp;

namespace YourNamespace.Filters
{
     
    public class RequireSigninFilter : IActionFilter
    {
        private readonly UserDbContext _dbContext;

        public RequireSigninFilter(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the user is authenticated
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // User is not authenticated, redirect to the login page
                context.Result = new RedirectResult("/Login/Login");
            }      
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed
        }
    }
}