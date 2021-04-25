using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MVCexample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );


            //var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(f => f.MediaType == "application/xml");
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
