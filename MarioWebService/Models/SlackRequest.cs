namespace MarioWebService.Models
{
    public class SlackRequest
    {
        public CommandType CommandType { get; set; }
        public string AuthorizationToken { get; set; }
        public string CommandText { get; set; }
        public string ChannelName { get; set; }
    }
}