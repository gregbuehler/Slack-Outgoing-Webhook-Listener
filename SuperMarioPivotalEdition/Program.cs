using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;

namespace SuperMarioPivotalEdition
{
    class Program
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.pivotaltracker.com") };
        static string apikey = "INSERT YOUR KEY HERE";
        static List<string> tasks = new List<string>()
        {
            "Create pull request: **Placeholder for URL**",
            "QA validation",
            "Demo to product owner",
            "Create CAB: **Placeholder for URL**",
            "Approve pull request",
            "Secure code review CAB",
            "Approve CAB",
            "Merge to Develop"
        };
        static void Main(string[] args)
        {
            StartPivotalListener();
            Console.ReadLine();
        }

        public static void PostPivotalTasks(string projectAndStory)
        {
            foreach (var task in tasks)
            {
                Thread.Sleep(1000);
                var str = "{\"description\":\"" + task + "\"}";
                var content = new StringContent(str);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("X-TrackerToken", apikey);
                client.PostAsync("/services/v5/projects/" + projectAndStory + "/tasks", content);
            }
        }

        public static async void StartPivotalListener()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://pareidoliaiscreated.org:1338/");
            listener.Start();
            while (true)
            {
                var context = await listener.GetContextAsync();
                var reader = new StreamReader(context.Request.InputStream);
                var form = HttpUtility.ParseQueryString(reader.ReadToEnd());
                reader.Close();

                Console.WriteLine(form["text"]);
                string responseBody = "";
                var forms = form["text"].ToLower().Split(':');
                if (forms[0] == "help")
                {
                    responseBody = "*add pivotal:Giant Beetle* creates a new Pivotal issue with name \"Giant Beetle\" using our team's default Pivotal template.";
                }
                else if (forms[0] == "add pivotal")
                {
                    var js = new JObject(
                    new JProperty("name", form["text"].Split(':')[1]),
                    new JProperty("tasks", new JArray(
                        new JObject(new JProperty("description", "Create pull request: **Placeholder for URL**")),
                        new JObject(new JProperty("description", "QA validation")),
                        new JObject(new JProperty("description", "Approve pull request")),
                        new JObject(new JProperty("description", "Create CAB: **Placeholder for URL**")),
                        new JObject(new JProperty("description", "Approve CAB")),
                        new JObject(new JProperty("description", "Merge to Develop")),
                        new JObject(new JProperty("description", "Demo to product owner")))));
                    var content = new StringContent(js.ToString());
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    content.Headers.Add("X-TrackerToken", apikey);
                    var resp = await client.PostAsync("/services/v5/projects/1517647/stories", content);
                    var respStr = await resp.Content.ReadAsStringAsync();
                    var dyn = JsonConvert.DeserializeObject<Story>(respStr);
                    responseBody = $"New Pivotal issue created!\n{dyn.url}";
                }
                else if (forms[0] == "add tasks")
                {
                    PostPivotalTasks(forms[1]);
                }
                using (var writer = new StreamWriter(context.Response.OutputStream))
                {
                    var json = new JObject(new JProperty("text", responseBody)).ToString();
                    writer.Write(json);
                }
            }
        }
    }
}