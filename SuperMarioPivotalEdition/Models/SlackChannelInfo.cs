using System.Collections.Generic;
using Newtonsoft.Json;

namespace SuperMarioPivotalEdition.Models
{
    class SlackChannelInfo
    {
        public string SlackChannelName;
        public int PivotalProjectId;
        public List<string> DefaultTaskDescriptions;

        public SlackChannelInfo(string slackChannelName, int pivotalProjectId, List<string> defaultTaskDescriptions)
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
