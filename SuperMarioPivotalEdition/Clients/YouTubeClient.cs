using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Clients
{
    class YouTubeClient
    {
        private HttpClient _client;
        private string _apiKey;
        private Random _random;

        public YouTubeClient(string apiKey)
        {
            _client = new HttpClient { BaseAddress = new Uri("https://www.googleapis.com") };
            _apiKey = apiKey;
            _random = new Random();
        }

        public YouTubeSearchResponse SearchFor(string searchTerms)
        {
            var str =_client.GetStringAsync($"/youtube/v3/search?part=snippet&maxResults=10&type=video&q={searchTerms}&key={_apiKey}").Result;
            var resp = JsonConvert.DeserializeObject<YouTubeSearchResponse>(str);
            return resp;
        }

        public string SearchForRandom(string searchTerms)
        {
            var resp = SearchFor(searchTerms).items.Select(i => $"https://www.youtube.com/watch?v={i.id.videoId}").ToList();
            var rand = resp[_random.Next(0, resp.Count)];
            return rand;
        }
    }
}
