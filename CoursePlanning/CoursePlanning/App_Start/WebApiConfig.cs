using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace CoursePlanning
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Modified the defaults to support the 'Root' controller
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "Root", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ApiByName",
                routeTemplate: "apibyname/{controller}/{action}/{code}",
                defaults: new { controller = "Root", code = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ApiAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Root", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ApiAdd",
                routeTemplate: "api/{controller}/{action}/{programId}",
                defaults: new { controller = "Root", programId = RouteParameter.Optional }
            );
        }
    }
}
