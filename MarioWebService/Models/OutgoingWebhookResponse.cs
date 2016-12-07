using Newtonsoft.Json;

namespace MarioWebService.Models
{
    public class OutgoingWebhookResponse
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}