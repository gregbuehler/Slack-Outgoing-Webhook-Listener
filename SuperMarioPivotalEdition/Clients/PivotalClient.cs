using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json.Linq;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Clients
{
    class PivotalClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public PivotalClient()
        {
            _client = new HttpClient {BaseAddress = new Uri("https://www.pivotaltracker.com")};
            _apiKey = ConfigurationManager.AppSettings["PivotalApiKey"];
        }

        public PivotalClientResponse Post(string resourceUri, JObject json)
        {
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("X-TrackerToken", _apiKey);
            var response = _client.PostAsync(resourceUri, content).Result;
            Thread.Sleep(1000);
            Console.WriteLine($"Posted to Pivotal:\nResource: {resourceUri}\nPayload: {json},\nPivotal Response: {response.Content.ReadAsStringAsync().Result}\n");
            var pivotalClientResponse = new PivotalClientResponse
            {
                IsSuccessful = response.IsSuccessStatusCode,
                HttpResponseMessage = response
            };
            return pivotalClientResponse;
        }

        public PivotalClientResponse PostTask(SlackChannelInfo slackChannelInfo, string storyId, string taskDescription)
        {
            var json = new JObject(new JProperty("description", taskDescription));
            var pivotalClientResponse =
                Post($"/services/v5/projects/{slackChannelInfo.PivotalProjectId}/stories/{storyId}/tasks", json);
            pivotalClientResponse.ShortResponseMessage = pivotalClientResponse.IsSuccessful
                ? $"Task \"{taskDescription}\" posted successfully."
                : "Error posting task.";
            return pivotalClientResponse;
        }

        public List<PivotalClientResponse> PostDefaultTasks(SlackChannelInfo slackChannelInfo, string storyId)
        {
            var responses = slackChannelInfo.DefaultTaskDescriptions.Select(taskDescription => PostTask(slackChannelInfo, storyId, taskDescription));
            return responses.ToList();
        }

        public PivotalClientResponse PostStory(SlackChannelInfo slackChannelInfo, string storyName)
        {
            var tasks = new JArray();
            foreach (var defaultTaskDescription in slackChannelInfo.DefaultTaskDescriptions)
            {
                tasks.Add(new JObject(new JProperty("description", defaultTaskDescription)));
            }
            var json = new JObject(
                new JProperty("name", storyName),
                new JProperty("tasks", tasks));
            var pivotalClientResponse = Post($"/services/v5/projects/{slackChannelInfo.PivotalProjectId}/stories", json);
            if (pivotalClientResponse.IsSuccessful)
            {
                dynamic dHttpResponse = JObject.Parse(pivotalClientResponse.HttpResponseMessage.Content.ReadAsStringAsync().Result);
                var url = dHttpResponse.url;
                pivotalClientResponse.ShortResponseMessage = $"Story created at {url}";
            }
            else
            {
                pivotalClientResponse.ShortResponseMessage = "Error creating story.";
            }
            return pivotalClientResponse;
        }
    }
}