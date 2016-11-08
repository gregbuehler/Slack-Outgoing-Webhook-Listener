using System;
using System.Configuration;
using System.Net.Http;
using System.Xml.Serialization;
using ApiIntegrations.Models.CatApi;

namespace ApiIntegrations.Clients
{
    public class CatApiClient : IClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client = new HttpClient {BaseAddress = new Uri("http://thecatapi.com")};

        public CatApiClient()
        {
            _apiKey = ConfigurationManager.AppSettings["CatApiKey"];
            ;
        }

        public CatApiResponse GetCats(int n)
        {
            var response = _client.GetAsync($"api/images/get?format=xml&results_per_page={n}&api_key={_apiKey}").Result;
            var xmlSerializer = new XmlSerializer(typeof(CatApiResponse), new XmlRootAttribute("response"));
            var stream = response.Content.ReadAsStreamAsync().Result;
            var res = (CatApiResponse) xmlSerializer.Deserialize(stream);
            return res;
        }

        public bool HealthCheck()
        {
            return !string.IsNullOrWhiteSpace(_apiKey);
        }
    }
}