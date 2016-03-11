﻿using System;
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
        private string _slackOutgoingWebhookToken;

        public SlackListener(DatabaseClient databaseClient, string pivotalApiKey, string slackOutgoingWebhookToken, string serverAddress)
        {
            _databaseClient = databaseClient;
            _pivotalClient = new PivotalClient(pivotalApiKey);
            _slackOutgoingWebhookToken = slackOutgoingWebhookToken;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(serverAddress);
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
                    Console.WriteLine($"Slack request received. Contents:\n\n{queryString}\n\n");
                    form = HttpUtility.ParseQueryString(queryString);
                }
                string responseBody;
                try
                {
                    responseBody = ProcessSlackCommand(form);
                }
                catch (Exception)
                {
                    responseBody = "SCREAMS OF DEATH";
                }
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
            var splitFormText = form["text"].Split(new[] {':'}, 2);
            var formTextContent = splitFormText.Length == 2 ? splitFormText[1] : "";
            var channel = form["channel_name"];
            var channelInfo = _databaseClient.GetChannelInfoFromChannelName(channel);
            var token = form["token"];
            var response = "";
            if (token != _slackOutgoingWebhookToken) return response;
            switch (triggerWord)
            {
                case "add story":
                    var storyTitle = formTextContent;
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
                    var storyId = formTextContent;
                    var httpResponseMessages = _pivotalClient.PostDefaultTasks(channelInfo, storyId);
                    var success = httpResponseMessages.Aggregate(true,
                        (b, message) => b && message.StatusCode == HttpStatusCode.OK);
                    response = success ? "Default tasks added." : "Error adding tasks";
                    break;
                case "add default task":
                    var taskDescription = formTextContent;
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
                    var projectId = formTextContent;
                    channelInfo.PivotalProjectId = projectId;
                    _databaseClient.WriteToDatabase(channelInfo);
                    response = $"Pivotal project ID set to {projectId}.";
                    break;
                case "set default tasks from json":
                    var json = formTextContent;
                    var jarray = JArray.Parse(json);
                    var taskList = jarray.ToObject<List<string>>();
                    channelInfo.DefaultTaskDescriptions = taskList;
                    _databaseClient.WriteToDatabase(channelInfo);
                    response = $"Default tasks set to:```{jarray}```";
                    break;
                case "info":
                    response = $"```{channelInfo}```";
                    break;
                case "help":
                    response = @"_All commands are case-insensitive_:
*help* - Displays command help.
*info* - Displays this channel's associated Pivotal info.
*set project id:123* - Sets this channel's associated Pivotal Project ID to 123.
*add tasks:12345* - Adds default tasks to story ID 12345.
*add story:Giant Beetle* - Creates a new Pivotal issue with name ""Giant Beetle"" with default tasks.
*add default task:Check exhaust ports* - Adds a new task to your team's default tasks.
*clear default tasks* - Clears default task list.
*set default tasks from json:*`[""task1"", ""task2""]` - Parses a JSON array and sets it as the default tasks. Useful for setting tasks all at once.";
                    break;
            }
            return response;
        }

    }
}
