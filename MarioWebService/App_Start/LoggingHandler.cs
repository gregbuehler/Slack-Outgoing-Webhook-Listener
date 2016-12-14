using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace MarioWebService
{
    public class LoggingHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoggingHandler));

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log.Debug($"Request body: {await request.Content.ReadAsStringAsync()}");
            var response = await base.SendAsync(request, cancellationToken);
            Log.Debug($"Response body: {await response.Content.ReadAsStringAsync()}");
            return response;
        }
    }
}