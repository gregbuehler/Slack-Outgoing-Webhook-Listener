using System.Collections.Generic;

namespace SuperMarioPivotalEdition
{
    class SlackChannelInfo
    {
        public string SlackChannelName;
        public string PivotalProjectId;
        public string PivotalApiKey;
        public List<string> DefaultTaskDescriptions;

        public SlackChannelInfo(string slackChannelName, string pivotalProjectId, string pivotalApiKey, List<string> defaultTaskDescriptions)
        {
            SlackChannelName = slackChannelName;
            PivotalProjectId = pivotalProjectId;
            PivotalApiKey = pivotalApiKey;
            DefaultTaskDescriptions = defaultTaskDescriptions;
        }
    }
}
