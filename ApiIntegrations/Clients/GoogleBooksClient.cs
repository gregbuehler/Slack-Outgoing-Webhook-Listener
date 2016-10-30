using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using ApiIntegrations.Models.GoogleBooks;
using Newtonsoft.Json;

namespace ApiIntegrations.Clients
{
    public class GoogleBooksClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private readonly Dictionary<string, string> _htmlTagsToConvert;
        private readonly Random _random;

        public GoogleBooksClient()
        {
            _client = new HttpClient {BaseAddress = new Uri("https://www.googleapis.com")};
            _apiKey = ConfigurationManager.AppSettings["GoogleApiKey"];
            _random = new Random();
            _htmlTagsToConvert = new Dictionary<string, string>
            {
                {"<b>", "*"},
                {"</b>", "*"},
                {"<br>", ""}
            };
        }

        private GoogleBooksResponse SearchFor(string text)
        {
            var responseString = _client.GetStringAsync($"/books/v1/volumes?q={text}&key={_apiKey}").Result;
            return JsonConvert.DeserializeObject<GoogleBooksResponse>(responseString);
        }

        public string SearchForAndReturnRandomTextSnippet(string text)
        {
            var gbr = SearchFor(text);
            if ((gbr?.items == null) || (gbr.items.Length <= 0)) return "No results found.";
            var randomResult = gbr.items[_random.Next(0, gbr.items.Length)];
            return ConvertHtmlTextToSlackCompatibleText(randomResult.ToString());
        }

        /// <summary>
        ///     Google books returns HTML, but Slack only supports a special Markdown-like format, so we have to convert it.
        /// </summary>
        /// <param name="htmlString">HTML to convert</param>
        /// <returns></returns>
        private string ConvertHtmlTextToSlackCompatibleText(string htmlString)
        {
            var htmlStringWithHtmlEntitiesConvertedToPlaintext = WebUtility.HtmlDecode(htmlString);
            var output = new StringBuilder(htmlStringWithHtmlEntitiesConvertedToPlaintext);
            foreach (var kvp in _htmlTagsToConvert)
                output.Replace(kvp.Key, kvp.Value);
            return output.ToString();
        }
    }
}