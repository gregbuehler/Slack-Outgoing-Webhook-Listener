using System.Web.Http;

namespace MarioWebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "",
                defaults: new
                {
                    action = "Endpoint",
                    controller = "Mario"
                }
            );
        }
    }
}
