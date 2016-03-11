using System.Collections.Generic;
using Newtonsoft.Json;

namespace SuperMarioPivotalEdition
{
    class SlackChannelInfo
    {
        public string SlackChannelName;
        public string PivotalProjectId;
        public List<string> DefaultTaskDescriptions;

        public SlackChannelInfo(string slackChannelName, string pivotalProjectId, List<string> defaultTaskDescriptions)
        {
            SlackChannelName = slackChannelName;
            PivotalProjectId = pivotalProjectId;
            DefaultTaskDescriptions = defaultTaskDescriptions;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
