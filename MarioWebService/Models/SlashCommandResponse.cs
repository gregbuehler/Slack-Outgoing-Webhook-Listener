using System.Collections.Generic;
using MarioWebService.Enums;
using Newtonsoft.Json;

namespace MarioWebService.Models
{
    public class SlashCommandResponse
    {
        [JsonProperty(PropertyName = "response_type")]
        public ResponseType ResponseType { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "attachments")]
        public List<Attachment> Attachments { get; set; }
    }
}