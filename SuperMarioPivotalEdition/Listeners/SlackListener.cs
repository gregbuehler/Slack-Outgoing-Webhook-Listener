using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SuperMarioPivotalEdition
{
    class SlackListener
    {
        HttpListener _httpListener;
        private DatabaseClient _databaseClient;
        private PivotalClient _pivotalClient;
        private GoogleCalendarClient _googleCalendarClient;

        public SlackListener(DatabaseClient databaseClient)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://pareidoliaiscreated.org:1338/");
            _httpListener.Start();
            _databaseClient = databaseClient;
            _pivotalClient = new PivotalClient(_databaseClient);
            _googleCalendarClient = new GoogleCalendarClient();
        }

        public async void ListenForSlackOutgoingWebhooks()
        {
            while (true)
            {
                Console.WriteLine("LISTENING");

                var context = await _httpListener.GetContextAsync();
                Console.WriteLine("GOT A HIT");
                NameValueCollection form;
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    form = HttpUtility.ParseQueryString(reader.ReadToEnd());
                }
                var responseBody = ProcessSlackCommand(form, _pivotalClient);
                using (var writer = new StreamWriter(context.Response.OutputStream))
                {
                    var json = new JObject(new JProperty("text", responseBody)).ToString();
                    writer.Write(json);
                }
            }
        }

        private string ProcessSlackCommand(NameValueCollection form, PivotalClient pivotalClient)
        {
            var triggerWord = form["trigger_word"].ToLower();
            var slackName = form["user_name"];
            var formText = form["text"];
            var channel = form["channel_name"];
            var channelInfo = _databaseClient.GetChannelInfoFromChannelName(channel);
            string response = "";
            switch (triggerWord)
            {
                case "add pivotal":
                    var storyDescription = formText.Split(':')[1];
                    pivotalClient.PostStory(channelInfo, storyDescription);
                    // Figure out if we want to add default team tasks later.
                    break;
                case "add tasks":
                    var storyId = formText.Split(':')[1];
                    var defaultTasks = channelInfo.DefaultTaskDescriptions;
                    pivotalClient.PostTasks(channelInfo, storyId, defaultTasks);
                    break;
                case "help":
                    response = "*add pivotal:Giant Beetle* creates a new Pivotal issue with name \"Giant Beetle\" using our team's default Pivotal template.";
                    break;
                case "check release tags":

                    break;
                case "update google calendar":

                    break;
            }
            return response;
        }

    }
}
