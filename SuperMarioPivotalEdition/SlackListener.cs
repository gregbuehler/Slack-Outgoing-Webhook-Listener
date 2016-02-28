using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
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

        public SlackListener(DatabaseClient databaseClient, PivotalClient pivotalClient, GoogleCalendarClient googleCalendarClient)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://pareidoliaiscreated.org:1338/");
            _httpListener.Start();
            _pivotalClient = pivotalClient;
            _googleCalendarClient = googleCalendarClient;
        }

        public async void ListenForSlackOutgoingWebhooks()
        {
            while (true)
            {
                var context = await _httpListener.GetContextAsync();
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
            var channel = form["channel LOOK THIS UP TO MAKE SURE"];
            var channelInfo = _databaseClient.GetChannelInfoFromChannelName(channel);
            string response = "";
            switch (triggerWord)
            {
                case "add pivotal":
                    var storyDescription = formText.Split(':')[1];
                    var projectId = channelInfo.PivotalProjectId;
                    pivotalClient.PostStory(projectId, storyDescription);
                    // Figure out if we want to add default team tasks later.
                    break;
                case "taskify":
                    var storyId = formText.Split(':')[1];
                    var defaultTasks = channelInfo.DefaultTaskDescriptions;
                    pivotalClient.PostTasks(storyId, defaultTasks);
                    break;
                case "help":
                    response = "*add pivotal:Giant Beetle* creates a new Pivotal issue with name \"Giant Beetle\" using our team's default Pivotal template.";
                    break;
                case "check release tags":

                    break;
            }
            return response;
        }

    }
}
