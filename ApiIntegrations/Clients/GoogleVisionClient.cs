using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using ApiIntegrations.Models.GoogleVision;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiIntegrations.Clients
{
    public class GoogleVisionClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private readonly HttpClient _genericClient;

        public GoogleVisionClient()
        {
            _client = new HttpClient {BaseAddress = new Uri("https://vision.googleapis.com")};
            _genericClient = new HttpClient();
            _apiKey = ConfigurationManager.AppSettings["GoogleApiKey"];
        }

        public GoogleVisionResponse Annotate(string imageUrl)
        {
            var imageStream = _genericClient.GetStreamAsync(imageUrl).Result;
            string base64ImageData;
            using (var memoryStream = new MemoryStream())
            {
                imageStream.CopyTo(memoryStream);
                base64ImageData = Convert.ToBase64String(memoryStream.ToArray());
            }
            var json =
                new JObject(new JProperty("requests",
                    new JArray(
                        new JObject(new JProperty("image", new JObject(new JProperty("content", base64ImageData))),
                            new JProperty("features",
                                new JArray(new JObject(new JProperty("type", "LABEL_DETECTION"),
                                    new JProperty("maxResults", 10))))))));
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var resp = _client.PostAsync($"/v1/images:annotate?key={_apiKey}", content).Result;
            var respStr = resp.Content.ReadAsStringAsync().Result;
            var gvr = JsonConvert.DeserializeObject<GoogleVisionResponse>(respStr);
            return gvr;
        }

        public string AnnotateAndReturnUrlOfBarchart(string imageUrl)
        {
            var gvr = Annotate(imageUrl);
            var values = gvr.responses[0].labelAnnotations.Select(d => d.score).ToList();
            var labels = gvr.responses[0].labelAnnotations.Select(d => d.description).ToList();
            var valuesUrlString = values.Aggregate("", (s, d) => s + (100*d).ToString("F1") + ",").Trim(',');
            var labelsUrlString = HttpUtility.UrlEncode(labels.Aggregate("", (s, s1) => s + s1 + "|").Trim('|'));
            return
                $"https://chart.googleapis.com/chart?cht=bhs&chs=500x300&chd=t:{valuesUrlString}&chxt=x,y&chxl=1:|{labelsUrlString}";
        }
    }
}