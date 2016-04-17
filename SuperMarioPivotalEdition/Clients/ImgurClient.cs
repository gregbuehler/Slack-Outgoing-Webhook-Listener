using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Clients
{
    class ImgurClient
    {
        private readonly HttpClient _apiClient;
        private readonly Random _random;

        public ImgurClient(string apiKey)
        {
            _apiClient = new HttpClient { BaseAddress = new Uri("https://api.imgur.com") };
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", apiKey);
            _random = new Random();
        }

        public ImgurResponse SearchFor(string searchTerms)
        {
            Console.WriteLine($"Search terms: {searchTerms}.");
            var resp = _apiClient.GetStringAsync($"/3/gallery/search/top?q={searchTerms}").Result;
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
