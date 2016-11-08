using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using ApiIntegrations.Models.Imgur;
using Newtonsoft.Json;

namespace ApiIntegrations.Clients
{
    public class ImgurClient : IClient
    {
        private readonly HttpClient _client;
        private readonly Random _random;
        private readonly string _apiKey;

        public ImgurClient()
        {
            _client = new HttpClient {BaseAddress = new Uri("https://api.imgur.com")};
            _apiKey = ConfigurationManager.AppSettings["ImgurApiKey"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", _apiKey);
            _random = new Random();
        }

        public ImgurResponse SearchFor(string searchTerms)
        {
            Console.WriteLine($"Search terms: {searchTerms}.");
            var resp = _client.GetStringAsync($"/3/gallery/search/top?q={searchTerms}").Result;
            return JsonConvert.DeserializeObject<ImgurResponse>(resp);
        }

        public string SearchForRandom(string searchTerms)
        {
            var ir = SearchFor(searchTerms);
            var irImages = ir.data.Where(d => !d.is_album && !d.nsfw).Select(d => d.link).ToList();
            var rand = irImages[_random.Next(0, irImages.Count)];
            return rand;
        }

        public bool HealthCheck()
        {
            return !string.IsNullOrWhiteSpace(_apiKey);
        }
    }
}