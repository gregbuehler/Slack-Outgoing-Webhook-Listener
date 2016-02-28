using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SuperMarioPivotalEdition
{
    class PivotalClient
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.pivotaltracker.com") };
        static string apikey = "GET THIS FROM FUCKING DATABASE, NO SOURCE CONTROL";

        public void Post(string resourceUri, JObject json)
        {
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("X-TrackerToken", apikey);
            var response = client.PostAsync(resourceUri, content).Result;
            Thread.Sleep(1000);
            var pivotalResponse = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Posted to Pivotal:\nResource: {resourceUri}\nPayload: {json}, Pivotal Response: {pivotalResponse}");
        }

        public void PostTask(string projectId, string storyId, string taskDescription)
        {
            var json = new JObject(new JProperty("description", taskDescription));
            Post($"/services/v5/projects/{projectId}/{storyId}/tasks", json);
        }

        public void PostTasks(string projectId, string storyId, List<string> taskDescriptions)
        {
            foreach (var taskDescription in taskDescriptions)
            {
                PostTask(projectId, storyId, taskDescription);
            }
        }

        public void PostStory(string projectId, string storyName)
        {
            var json = new JObject(new JProperty("name", storyName),
                new JProperty("tasks", new JArray()));
            Post($"/services/v5/projects/{projectId}/stories", json);
        }
        // Might want to pass in ChannelInfo objects instead of strings and List<strings>
        // since that already contains the info. Herein lies the road
        // to ruination.

        public void CheckReleaseTags(string projectId)
        {
            
        }
    }
}
