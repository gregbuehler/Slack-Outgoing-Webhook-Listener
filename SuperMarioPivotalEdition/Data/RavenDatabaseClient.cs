using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Raven.Client.Document;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Data
{
    internal class RavenDatabaseClient : IDatabaseClient
    {
        private readonly DocumentStore _documentStore;

        public RavenDatabaseClient()
        {
            _documentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = ConfigurationManager.AppSettings["RavenDatabaseName"]
            };
            _documentStore.Initialize();
        }

        public void UpdateSlackChannelInfo(SlackChannelInfo slackChannelInfo)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(slackChannelInfo, slackChannelInfo.SlackChannelName);
                session.SaveChanges();
            }
        }

        public SlackChannelInfo GetSlackChannelInfo(string slackChannelName)
        {
            using (var session = _documentStore.OpenSession())
            {
                var channelInfo =
                    session.Query<SlackChannelInfo>().FirstOrDefault(x => x.SlackChannelName == slackChannelName);
                if (channelInfo != null) return channelInfo;
                channelInfo = new SlackChannelInfo(slackChannelName, 0, new List<string>());
                UpdateSlackChannelInfo(channelInfo);
                return channelInfo;
            }
        }
    }
}