using System.Collections.Generic;
using System.Linq;
using Raven.Client.Document;
using SuperMarioPivotalEdition.Models;
using SuperMarioPivotalEdition.Data;

namespace SuperMarioPivotalEdition.Clients
{
    class RavenDatabaseClient : IDatabaseClient
    {
        private readonly DocumentStore _documentStore;

        public RavenDatabaseClient(string databaseName)
        {
            _documentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = databaseName
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
                var channelInfo = session.Query<SlackChannelInfo>().FirstOrDefault(x => x.SlackChannelName == slackChannelName);
                if (channelInfo != null) return channelInfo;
                channelInfo = new SlackChannelInfo(slackChannelName, "", new List<string>());
                UpdateSlackChannelInfo(channelInfo);
                return channelInfo;
            }
        }
    }
}