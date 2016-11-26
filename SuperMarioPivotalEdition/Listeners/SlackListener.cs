using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using log4net;
using Newtonsoft.Json.Linq;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Listeners
{
    internal class SlackListener
    {
        private readonly HttpListener _httpListener;
        private readonly SlackCommandProcessor _slackCommandProcessor;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SlackListener));

        public SlackListener()
        {
            _slackCommandProcessor = new SlackCommandProcessor();
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(ConfigurationManager.AppSettings["ServerAddress"]);
            _httpListener.Start();
        }

        public async void ListenForSlackOutgoingWebhooks()
        {
            while (true)
            {
                var context = await _httpListener.GetContextAsync();
                NameValueCollection form;
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var queryString = reader.ReadToEnd();
                    Log.Debug($"Slack request received. Contents:\n\n{queryString}\n\n");
                    form = HttpUtility.ParseQueryString(queryString);
                }
                string responseBody;
                try
                {
                    responseBody = _slackCommandProcessor.Process(form);
                }
                catch (Exception ex)
                {
                    responseBody = "SCREAMS OF DEATH";
                    Log.Error("Error processing command.", ex);
                }
                using (var writer = new StreamWriter(context.Response.OutputStream))
                {
                    var json = new JObject(new JProperty("text", responseBody)).ToString();
                    writer.Write(json);
                }
            }
        }
    }
}