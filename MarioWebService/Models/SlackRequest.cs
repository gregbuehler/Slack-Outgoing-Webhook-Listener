namespace MarioWebService.Models
{
    public class SlackRequest
    {
        public string Token { get; set; }
        public string Command { get; set; }
        public string Text { get; set; }
        public string ChannelName { get; set; }
    }
}