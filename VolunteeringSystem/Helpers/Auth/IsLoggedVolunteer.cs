using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Admin.Helpers
{
    public class IsLoggedVolunteerAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var volunteerId = context.HttpContext.Session.GetString("volunteerId");
            var type = context.HttpContext.Session.GetString("type");

            if (string.IsNullOrEmpty(volunteerId) || type != "VOLUNTEER")
            {
                context.Result = new RedirectResult("~/Volunteer/Login");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To do : after the action executes  
        }
    }
}