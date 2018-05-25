using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Admin.Helpers
{
    public class IsLoggedAdminAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetString("user");
            var type = context.HttpContext.Session.GetString("type");

            if (string.IsNullOrEmpty(user) || type != "ADMIN") context.Result = new RedirectResult("~/Admin/Login");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}