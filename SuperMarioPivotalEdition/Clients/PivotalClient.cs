using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace SuperMarioPivotalEdition
{
    class PivotalClient
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.pivotaltracker.com") };
        private DatabaseClient _databaseClient;

        public PivotalClient(DatabaseClient databaseClient)
        {
            _databaseClient = databaseClient;
        }

        public void Post(string resourceUri, JObject json, string apiKey)
        {
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("X-TrackerToken", apiKey);
            var response = client.PostAsync(resourceUri, content).Result;
            Thread.Sleep(1000);
            var pivotalResponse = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Posted to Pivotal:\nResource: {resourceUri}\nPayload: {json}, Pivotal Response: {pivotalResponse}");
        }

        public void PostTask(SlackChannelInfo slackChannelInfo, string storyId, string taskDescription)
        {
            var json = new JObject(new JProperty("description", taskDescription));
            Post($"/services/v5/projects/{slackChannelInfo.PivotalProjectId}/{storyId}/tasks", json, slackChannelInfo.PivotalApiKey);
        }

        public void PostTasks(SlackChannelInfo slackChannelInfo, string storyId, List<string> taskDescriptions)
        {
            foreach (var taskDescription in taskDescriptions)
            {
                PostTask(slackChannelInfo, storyId, taskDescription);
            }
        }

        public void PostStory(SlackChannelInfo slackChannelInfo, string storyName)
        {
            var json = new JObject(new JProperty("name", storyName),
                new JProperty("tasks", new JArray()));
            Post($"/services/v5/projects/{slackChannelInfo.PivotalProjectId}/stories", json, slackChannelInfo.PivotalApiKey);
        }
        // Might want to pass in SlackChannelInfo objects instead of strings and List<strings>
        // since that already contains the info. Herein lies the road
        // to ruination.

        public void CheckReleaseTags(string projectId)
        {
            
        }
    }
}
