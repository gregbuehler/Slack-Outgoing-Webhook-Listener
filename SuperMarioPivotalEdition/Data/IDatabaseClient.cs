using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Data
{
    interface IDatabaseClient
    {
        void UpdateSlackChannelInfo(SlackChannelInfo slackChannelInfo);
        SlackChannelInfo GetSlackChannelInfo(string slackChannelName);
    }
}
