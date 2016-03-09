using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
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
            _pivotalClient = new PivotalClient();
            _googleCalendarClient = new GoogleCalendarClient();
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
                var responseBody = ProcessSlackCommand(form);
                using (var writer = new StreamWriter(context.Response.OutputStream))
                {
                    var json = new JObject(new JProperty("text", responseBody)).ToString();
                    writer.Write(json);
                }
            }
        }

        private string ProcessSlackCommand(NameValueCollection form)
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
                    var storyTitle = formText.Split(':')[1];
                    var httpResponseMessage = _pivotalClient.PostStory(channelInfo, storyTitle);
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        dynamic dHttpResponse = JObject.Parse(httpResponseMessage.Content.ReadAsStringAsync().Result);
                        var url = dHttpResponse.url;
                        response = $"Story created at {url}";
                    }
                    else
                    {
                        response = "Error creating story.";
                    }
                    break;
                case "add tasks":
                    var storyId = formText.Split(':')[1];
                    var httpResponseMessages = _pivotalClient.PostDefaultTasks(channelInfo, storyId);
                    var success = httpResponseMessages.Aggregate(true,
                        (b, message) => b && message.StatusCode == HttpStatusCode.OK);
                    response = success ? "Default tasks added." : "Error adding tasks";
                    break;
                case "add default task":
                    var taskDescription = formText.Split(':')[1];
                    channelInfo.DefaultTaskDescriptions.Add(taskDescription);
                    _databaseClient.WriteToDatabase(channelInfo);
                    break;
                case "help":
                    response = @"Command info:
*add pivotal:Giant Beetle* creates a new Pivotal issue with name ""Giant Beetle"" and adds default tasks.
*add tasks:12345* adds default tasks to story ID 12345.
*add default task:Check exhaust ports* adds a new task to your team's default tasks.";
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
