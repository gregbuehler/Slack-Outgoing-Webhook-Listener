using System;
using System.Net.Http;
using System.Xml.Serialization;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Clients
{

    class CatApiClient
    {
        private HttpClient _client = new HttpClient { BaseAddress = new Uri("http://thecatapi.com") };
        private string _apiKey;

        public CatApiClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public CatApiResponse GetCats(int n)
        {
            var response = _client.GetAsync($"api/images/get?format=xml&results_per_page={n}&api_key={_apiKey}").Result;
            var xmlSerializer = new XmlSerializer(typeof(CatApiResponse), new XmlRootAttribute("response"));
            var stream = response.Content.ReadAsStreamAsync().Result;
            var res = (CatApiResponse)xmlSerializer.Deserialize(stream);
            return res;
        }
    }
}
