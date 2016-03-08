using System.Linq;
using Raven.Client.Document;

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

        public void WriteToDatabase(object obj)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(obj);
                session.SaveChanges();
            }
        }

        public SlackChannelInfo GetChannelInfoFromChannelName(string slackChannelName)
        {
            using (var session = _documentStore.OpenSession())
            {
                var channelInfo = session.Query<SlackChannelInfo>().First(x => x.SlackChannelName == slackChannelName);
                return channelInfo;
            }
        }



    }
}
