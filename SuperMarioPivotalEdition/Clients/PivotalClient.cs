using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SuperMarioPivotalEdition.Models.Pivotal;

namespace SuperMarioPivotalEdition.Clients
{
    class PivotalClient
    {
        private readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://www.pivotaltracker.com")
        };
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        public PivotalClient()
        {
            var apiKey = ConfigurationManager.AppSettings["PivotalApiKey"];
            _client.DefaultRequestHeaders.Add("X-TrackerToken", apiKey);
        }

        private T Post<T>(string resourceUri, T content)
        {
            var c = new StringContent(JsonConvert.SerializeObject(content, _jsonSerializerSettings))
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };
            var response = _client.PostAsync($"services/v5/{resourceUri}", c).Result;
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result, _jsonSerializerSettings);
            throw new Exception($"POST to Pivotal:\nResource: {resourceUri}\nPayload: {content}\nPivotal Response: {response.Content.ReadAsStringAsync().Result}\n");
        }

        private T Get<T>(string resourceUri)
        {
            var response = _client.GetAsync($"services/v5/{resourceUri}").Result;
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result, _jsonSerializerSettings);
            throw new Exception($"GET to Pivotal:\nResource: {resourceUri}\nPivotal Response: {response.Content.ReadAsStringAsync().Result}\n");
        }

        public Project[] GetProjects()
        {
            return Get<Project[]>("projects"); ;
        }

        public Story GetStory(Story story)
        {
            return Get<Story>($"projects/{story.project_id}/stories/{story.id}");
        }

        public Story PostStory(Story story)
        {
           return Post($"projects/{story.project_id}/stories", story);
        }

        public Task PostTask(Story story, Task task)
        {
            return Post($"projects/{story.project_id}/stories/{story.id}/tasks", task);
        }

        public Task[] PostTasks(Story story, Task[] tasks)
        {
            return tasks.Select(t => PostTask(story, t)).ToArray();
        }

        public Task[] PostTasksWithProjectIdSafetyCheck(Story story, Task[] tasks)
        {
            try
            {
                return PostTasks(story, tasks);
            }
            catch (Exception)
            {
                foreach (var project in GetProjects())
                {
                    try
                    {
                        story.project_id = project.id;
                        story = GetStory(story);
                        return PostTasks(story, tasks);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                throw new Exception($"Could not find project ID for story ID #{story.id}, even after exhaustive search through projects available to this user.");
            }
        }
    }
}