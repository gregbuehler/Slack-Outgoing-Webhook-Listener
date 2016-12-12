using System.Web.Http;
using MarioWebService.Mappers;
using Newtonsoft.Json;

namespace MarioWebService
{
    public class MarioApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomJsonConverter());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
