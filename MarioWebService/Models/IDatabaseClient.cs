namespace MarioWebService.Models
{
    internal interface IDatabaseClient
    {
        void UpdateSlackChannelInfo(SlackChannelInfo slackChannelInfo);
        SlackChannelInfo GetSlackChannelInfo(string slackChannelName);
    }
}