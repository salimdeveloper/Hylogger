using Hylogger.web.Attributes;
using Hylogger.web.Services;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace HyApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Filters.Add(new ApiLoggerAttribute("crxClientPortal"));

            config.Services.Replace(typeof(IExceptionHandler), new CustomApiExceptionHandler());
            config.Services.Add(typeof(IExceptionLogger), new CustomApiExceptionLogger("crxClientPortal"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
