using System;
using System.Configuration;
using System.Net.Http;
using System.Web;

namespace ApiIntegrations.Clients
{
    public class BitlyClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public BitlyClient()
        {
            _client = new HttpClient {BaseAddress = new Uri("https://api-ssl.bitly.com")};
            _apiKey = ConfigurationManager.AppSettings["BitlyApiKey"];
        }

        public string ShortenUrl(string url)
        {
            var urlEncoded = HttpUtility.UrlEncode(url);
            var urlShortened =
                _client.GetStringAsync($"/v3/shorten?access_token={_apiKey}&longUrl={urlEncoded}&format=txt").Result;
            return urlShortened;
        }
    }
}