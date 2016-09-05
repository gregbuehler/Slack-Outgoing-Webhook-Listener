using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Clients
{
    class GoogleBooksClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly Random _random;
        private readonly Dictionary<string, string> _htmlTagsToConvert;

        public GoogleBooksClient(string apiKey)
        {
            _client = new HttpClient() { BaseAddress = new Uri("https://www.googleapis.com") };
            _apiKey = apiKey;
            _random = new Random();
            _htmlTagsToConvert = new Dictionary<string, string>()
            {
                {"<b>", "*"},
                {"</b>", "*"},
                {"<br>" , ""}
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
            var htmlsnippets = gbr?.items?.Select(i => i?.searchInfo?.textSnippet).Where(t => t != null).ToList();
            if (htmlsnippets == null) return "No results found.";
            var randomSnippet = htmlsnippets[_random.Next(0, htmlsnippets.Count)];
            return ConvertHtmlTextToSlackCompatibleText(randomSnippet);
        }

        /// <summary>
        /// Google books returns HTML, but Slack only supports a special Markdown-like format, so we have to convert it.
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
