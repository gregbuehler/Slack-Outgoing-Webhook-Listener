using System;
using System.Net.Http;
using System.Web;

namespace SuperMarioPivotalEdition.Clients
{
    class BitlyClient
    {
        private HttpClient _client;
        private string _apiKey;

        public BitlyClient(string apiKey)
        {
            _client = new HttpClient { BaseAddress = new Uri("https://api-ssl.bitly.com") };
            _apiKey = apiKey;
        }

        public string ShortenUrl(string url)
        {
            var urlEncoded = HttpUtility.UrlEncode(url);
            var urlShortened = _client.GetStringAsync($"/v3/shorten?access_token={_apiKey}&longUrl={urlEncoded}&format=txt").Result;
            return urlShortened;
        }
    }
}
