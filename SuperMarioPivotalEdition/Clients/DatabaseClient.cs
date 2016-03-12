using System.Linq;
using Raven.Client.Document;
using System.Collections.Generic;

namespace SuperMarioPivotalEdition
{
    class DatabaseClient
    {
        private DocumentStore _documentStore;

        public DatabaseClient(string databaseName)
        {
            _documentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = databaseName
            };
            _documentStore.Initialize();
        }

        public void WriteToDatabase(SlackChannelInfo slackChannelInfo)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(slackChannelInfo, slackChannelInfo.SlackChannelName);
                session.SaveChanges();
            }
        }

        public SlackChannelInfo GetChannelInfoFromChannelName(string slackChannelName)
        {
            using (var session = _documentStore.OpenSession())
            {
                var channelInfo = session.Query<SlackChannelInfo>().FirstOrDefault(x => x.SlackChannelName == slackChannelName);
                if (channelInfo != null) return channelInfo;
                channelInfo = new SlackChannelInfo(slackChannelName, "", new List<string>());
                WriteToDatabase(channelInfo);
                return channelInfo;
            }
        }
    }
}