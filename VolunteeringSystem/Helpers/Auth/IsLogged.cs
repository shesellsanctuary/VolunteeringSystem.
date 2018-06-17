using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Admin.Helpers
{
    public class IsLoggedAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectResult("~/");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To do : after the action executes  
        }
    }
}