using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SuperMarioPivotalEdition.Models.GitHub;

namespace SuperMarioPivotalEdition.Clients
{
    class GitHubClient
    {
        private readonly HttpClient _client;
        private readonly string _organization;

        public GitHubClient()
        {
            var apiKey = ConfigurationManager.AppSettings["GitHubApiKey"];
            _organization = ConfigurationManager.AppSettings["GitHubOrganization"];
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com"),
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey)))
                }
            };
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("C# HttpClient");
        }

        public Repository[] GetRepos()
        {
            return JsonConvert.DeserializeObject<Repository[]>(_client.GetAsync("/user/repos").Result.Content.ReadAsStringAsync().Result);
        }

        public Repository[] GetOrgRepos()
        {
            return JsonConvert.DeserializeObject<Repository[]>(_client.GetAsync($"/orgs/{_organization}/repos").Result.Content.ReadAsStringAsync().Result);
        }

        public string GetUrlToCodeSearchRepos(Repository[] repos, string codeSnippet)
        {
            var uri = new StringBuilder($"https://github.com/search?utf8=%E2%9C%93&type=Code&ref=searchresults&q={HttpUtility.UrlEncode(codeSnippet)}");
            foreach (var repository in repos)
            {
                uri.Append($"+repo%3A{HttpUtility.UrlEncode(repository.full_name)}");
            }
            return uri.ToString();
        }

        public string GetUrlToCodeSearchOrganizationRepos(string codeSnippet)
        {
            return GetUrlToCodeSearchRepos(GetOrgRepos(), codeSnippet);
        }
    }
}
