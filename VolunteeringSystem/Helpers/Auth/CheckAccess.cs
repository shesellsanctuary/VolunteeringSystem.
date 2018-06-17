using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Admin.Helpers
{
    public class CheckAccessAttribute : TypeFilterAttribute
    {
        public CheckAccessAttribute(params string[] userTypes) : base(typeof(CheckAccessImpl))
        {
            Arguments = new object[] { userTypes };
        }

        private class CheckAccessImpl : IActionFilter
        {
            private readonly string[] _userTypes;
            private readonly ILogger _logger;

            public CheckAccessImpl(ILoggerFactory loggerFactory, string[] userTypes)
            {
                _userTypes = userTypes;
                _logger = loggerFactory.CreateLogger<CheckAccessAttribute>();
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                string userType = context.HttpContext.Session.GetString("type");
                if (_userTypes.Length > 0)
                {
                    if (!_userTypes.Any(t => userType.Contains(t)))
                        context.Result = new RedirectResult("/Home/Denied");
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}
