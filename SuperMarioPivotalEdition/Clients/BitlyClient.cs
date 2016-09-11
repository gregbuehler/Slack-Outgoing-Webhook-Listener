using System;
using System.Configuration;
using System.Net.Http;
using System.Web;

namespace SuperMarioPivotalEdition.Clients
{
    class BitlyClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public BitlyClient()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://api-ssl.bitly.com") };
            _apiKey = ConfigurationManager.AppSettings["BitlyApiKey"];
        }

        public string ShortenUrl(string url)
        {
            var urlEncoded = HttpUtility.UrlEncode(url);
            var urlShortened = _client.GetStringAsync($"/v3/shorten?access_token={_apiKey}&longUrl={urlEncoded}&format=txt").Result;
            return urlShortened;
        }
    }
}
