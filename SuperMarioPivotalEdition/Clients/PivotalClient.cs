using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace SuperMarioPivotalEdition
{
    class PivotalClient
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.pivotaltracker.com") };

        public HttpResponseMessage Post(string resourceUri, JObject json, string apiKey)
        {
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("X-TrackerToken", apiKey);
            var response = client.PostAsync(resourceUri, content).Result;
            Thread.Sleep(1000);
            var pivotalResponse = response;
            Console.WriteLine($"Posted to Pivotal:\nResource: {resourceUri}\nPayload: {json},\nPivotal Response: {pivotalResponse.Content.ReadAsStringAsync().Result}");
            return pivotalResponse;
        }

        public HttpResponseMessage PostTask(SlackChannelInfo slackChannelInfo, string storyId, string taskDescription)
        {
            var json = new JObject(new JProperty("description", taskDescription));
            return Post($"/services/v5/projects/{slackChannelInfo.PivotalProjectId}/stories/{storyId}/tasks", json, slackChannelInfo.PivotalApiKey);
        }

        public List<HttpResponseMessage> PostDefaultTasks(SlackChannelInfo slackChannelInfo, string storyId)
        {
            var responses = slackChannelInfo.DefaultTaskDescriptions.Select(taskDescription => PostTask(slackChannelInfo, storyId, taskDescription));
            return responses.ToList();
        }

        public HttpResponseMessage PostStory(SlackChannelInfo slackChannelInfo, string storyName)
        {
            var tasks = new JArray();
            foreach (var defaultTaskDescription in slackChannelInfo.DefaultTaskDescriptions)
            {
                tasks.Add(new JProperty("description", defaultTaskDescription));
            }
            var json = new JObject(
                new JProperty("name", storyName),
                new JProperty("tasks", tasks));
            return Post($"/services/v5/projects/{slackChannelInfo.PivotalProjectId}/stories", json, slackChannelInfo.PivotalApiKey);
        }

        public void CheckReleaseTags(string projectId)
        {
            
        }
    }
}
