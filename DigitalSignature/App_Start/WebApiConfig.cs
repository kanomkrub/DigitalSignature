using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DigitalSignatureService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}", new { action = RouteParameter.Optional });


            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Add
            //    (new Core.Converter.PDFFieldConverter());
            // Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
