using Hylogger.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Hylogger.web.Attributes
{
    public class ApiLoggerAttribute : ActionFilterAttribute
    {
        private string _productName;

        public ApiLoggerAttribute(string productName)
        {
            _productName = productName;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var dict = new Dictionary<string, object>();

            string userId, userName;
            var user = actionContext.RequestContext.Principal as ClaimsPrincipal;
            Helpers.GetUserData(dict, user, out userId, out userName);

            string location;
            Helpers.GetLocationForApiCall(actionContext.RequestContext, dict, out location);

            actionContext.Request.Properties["PerfTracker"] = new PerfTracker(location,
                userId, userName, location, _productName, "API", dict);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var perfTracker = actionExecutedContext.Request.Properties["PerfTracker"]
                    as PerfTracker;

                if (perfTracker != null)
                    perfTracker.Stop();
            }
            catch (Exception)
            {
                // ignoring logging exceptions -- don't want an API call to fail 
                // if we run into logging problems. 
            }
        }
    }
}
