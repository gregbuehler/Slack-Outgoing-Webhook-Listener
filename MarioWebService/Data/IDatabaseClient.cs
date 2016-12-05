using MarioWebService.Models;

namespace MarioWebService.Data
{
    internal interface IDatabaseClient
    {
        void UpdateSlackChannelInfo(SlackChannelInfo slackChannelInfo);
        SlackChannelInfo GetSlackChannelInfo(string slackChannelName);
    }
}