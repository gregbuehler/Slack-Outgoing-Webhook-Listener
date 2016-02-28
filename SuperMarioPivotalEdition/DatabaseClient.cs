using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

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

        public string GetProjectIdFromChannelName(string slackChannelName)
        {
            using (var session = _documentStore.OpenSession())
            {
                var projectId = session.Query<ChannelInfo>().Where(x => x.SlackChannelName = slackChannelName).Select(x => x.PivotalProjectId).First();
                return projectId;
            }
        }

        public ChannelInfo GetChannelInfoFromChannelName(string slackChannelName)
        {
            using (var session = _documentStore.OpenSession())
            {
                var channelInfo = session.Query<ChannelInfo>().Where(x => x.SlackChannelName = slackChannelName).First();
                return channelInfo;
            }
        }



    }
}
