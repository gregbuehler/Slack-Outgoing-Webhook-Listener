using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
                    response = "New default task added.";
                    break;
                case "clear default tasks":
                    channelInfo.DefaultTaskDescriptions = new List<string>();
                    _databaseClient.WriteToDatabase(channelInfo);
                    response = "Default task list cleared.";
                    break;
                case "set project id":
                    var projectId = formText.Split(':')[1];
                    channelInfo.PivotalProjectId = projectId;
                    _databaseClient.WriteToDatabase(channelInfo);
                    response = $"Pivotal project ID set to {projectId}.";
                    break;
                case "set api key":
                    var apiKey = formText.Split(':')[1];
                    channelInfo.PivotalApiKey = apiKey;
                    _databaseClient.WriteToDatabase(channelInfo);
                    response = $"API Key set to {apiKey}.";
                    break;
                case "info":
                    response = $"```{channelInfo}```";
                    break;
                case "help":
                    response = @"_All commands are case-insensitive_:
*help* displays command help.
*add pivotal:Giant Beetle* creates a new Pivotal issue with name ""Giant Beetle"" with default tasks.
*add tasks:12345* adds default tasks to story ID 12345.
*add default task:Check exhaust ports* adds a new task to your team's default tasks.
*clear default tasks* clears default task list.
*set project id:123* sets this channel's associated Pivotal Project ID to 123.
*set api key:a1b2c3* sets this channel's Pivotal API Key to a1b2c3.
*info* displays this channel's associated Pivotal info.";
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
