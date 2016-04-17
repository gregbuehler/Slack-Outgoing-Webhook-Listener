using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Clients
{
    class GoogleVisionClient
    {
        private HttpClient _client;
        private HttpClient _genericClient;
        private string _apiKey;

        public GoogleVisionClient(string apiKey)
        {
            _client = new HttpClient { BaseAddress = new Uri("https://vision.googleapis.com") };
            _genericClient = new HttpClient();
            _apiKey = apiKey;
        }

        public string Annotate(string url)
        {
            var imageStream = _genericClient.GetStreamAsync(url).Result;
            string base64ImageData;
            using (var memoryStream = new MemoryStream())
            {
                imageStream.CopyTo(memoryStream);
                base64ImageData = Convert.ToBase64String(memoryStream.ToArray());
            }
            var json = new JObject(new JProperty("requests", new JArray(new JObject(new JProperty("image", new JObject(new JProperty("content", base64ImageData))), new JProperty("features", new JArray(new JObject(new JProperty("type", "LABEL_DETECTION"), new JProperty("maxResults", 10))))))));
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var resp = _client.PostAsync($"/v1/images:annotate?key={_apiKey}", content).Result;
            var respStr = resp.Content.ReadAsStringAsync().Result;
            return respStr;
        }

        
    }
}
