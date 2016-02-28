using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioPivotalEdition
{
    class ChannelInfo
    {
        public string ChannelName;
        public string PivotalProjectId;
        public List<string> DefaultTaskDescriptions;

        public ChannelInfo(string channelName, string pivotalProjectId, List<string> defaultTaskDescriptions)
        {
            ChannelName = channelName;
            PivotalProjectId = pivotalProjectId;
            DefaultTaskDescriptions = defaultTaskDescriptions;
        }
    }
}
