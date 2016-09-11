using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Clients
{
    class ImgurClient
    {
        private readonly HttpClient _client;
        private readonly Random _random;

        public ImgurClient()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://api.imgur.com") };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", ConfigurationManager.AppSettings["ImgurApiKey"]);
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


    }
}
