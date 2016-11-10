using System.Collections.Generic;
using Newtonsoft.Json;

namespace MarioWebService.Models
{
    internal class SlackChannelInfo
    {
        public List<string> DefaultTaskDescriptions;
        public int PivotalProjectId;
        public string SlackChannelName;

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