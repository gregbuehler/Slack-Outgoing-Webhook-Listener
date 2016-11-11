using System.Collections.Specialized;
using System.Web.Http;
using MarioWebService.Models;

namespace MarioWebService.Controllers
{
    public class MarioController : ApiController
    {
        private static readonly SlackCommandProcessor Processor = new SlackCommandProcessor();
        [HttpPost]
        public string Endpoint(NameValueCollection form)
        {
            string response;
            try
            {
                response = Processor.Process(form);
            }
            catch
            {
                response = "SCREAMS OF DEATH";
            }
            return response;
        }
    }
}