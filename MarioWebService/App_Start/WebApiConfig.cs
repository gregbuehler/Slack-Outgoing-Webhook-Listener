using System.Web.Http;

namespace MarioWebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "SlashCommand",
                defaults: new
                {
                    action = "SlashCommand",
                    controller = "Mario"
                }
            );
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "OutgoingWebhook",
                defaults: new
                {
                    action = "OutgoingWebhook",
                    controller = "Mario"
                }
            );
            config.MessageHandlers.Add(new LoggingHandler());
        }
    }
}
