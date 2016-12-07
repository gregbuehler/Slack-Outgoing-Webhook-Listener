using Newtonsoft.Json;

namespace MarioWebService.Models
{
    public class Attachment
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}