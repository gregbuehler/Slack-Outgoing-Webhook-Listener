using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Data
{
    internal interface IDatabaseClient
    {
        void UpdateSlackChannelInfo(SlackChannelInfo slackChannelInfo);
        SlackChannelInfo GetSlackChannelInfo(string slackChannelName);
    }
}